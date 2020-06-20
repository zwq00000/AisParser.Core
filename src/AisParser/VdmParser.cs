using System;
using System.Buffers;
using System.Text;

namespace AisParser {
    public static class VdmParser {

        static ReadOnlySpan<byte> NewLine => new byte[] {
            (byte)
            '\r', (byte)
            '\n'
        };

        private readonly static byte[] fieldSpliter = new byte[] {
            (byte)
            ',', (byte)
            '*'
        };

        public static VdmStatus ParseLine (ref ReadOnlySequence<byte> line, ref VdmVdo vdm, ref SixbitSpan sixbits) {
            var lineReader = new SequenceReader<byte> (line);
            Nmea.FindStart (ref lineReader);
            if (Nmea.CheckChecksum (ref line) != 0) {
                return VdmStatus.ChecksumFailed;
            }

            if (!TryReadFields (ref lineReader, out vdm)) {
                return VdmStatus.NotAisMessage;
            }
            if (!IsVdmOrVdo (vdm.Tag)) {
                //throw new VDMSentenceException ($"{vdm.Tag} Is Not a VDM or VDO message");
                return VdmStatus.NotAisMessage;
            }

            if (TryReadField (ref lineReader, out ReadOnlySpan<byte> span)) {
                sixbits.Add (span);
            } else {
                return VdmStatus.OutofSequence;
            }

            if (vdm.Total == 0 || vdm.Total == vdm.Num) {
                // Get the message id
                try {
                    vdm.MsgId = (int) sixbits.Get (6);
                } catch (SixbitsExhaustedException) {
                    throw new VDMSentenceException ("Not enough bits for msgid");
                }

                TryReadField (ref lineReader, out int padbits);
                // Adjust bit count
                sixbits.PadBits (padbits);
                // Found a complete packet
                return VdmStatus.Complete;
            }

            return VdmStatus.Incomplete;

        }

        public static bool TryParse (ref ReadOnlySequence<byte> buffer, out VdmVdo vdm, out SixbitSpan sixbits) {
            var reader = new SequenceReader<byte> (buffer);
            vdm = new VdmVdo ();
            sixbits = new SixbitSpan ();

            while (reader.TryReadTo (out var line, NewLine, true)) {
                var status = ParseLine (ref line, ref vdm,ref sixbits);
                switch(status){
                    case VdmStatus.Complete:
                    return true;
                    case VdmStatus.Incomplete:
                    continue;
                    default:
                    return false;
                }
            }

            // No complete message yet
            return false;
        }

public static bool TryParse (ref SequenceReader<byte> reader, out VdmVdo vdm, out SixbitSpan sixbits) {
            vdm = new VdmVdo ();
            sixbits = new SixbitSpan ();

            while (reader.TryReadTo (out var line, NewLine, true)) {
                var status = ParseLine (ref line, ref vdm,ref sixbits);
                switch(status){
                    case VdmStatus.Complete:
                    return true;
                    case VdmStatus.Incomplete:
                    continue;
                    default:
                    return false;
                }
            }

            // No complete message yet
            return false;
        }
        /// <summary>
        /// 尝试读取 VDM/VDO 字段
        /// tag/total/num/sequence/channel
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="vdm"></param>
        /// <returns></returns>
        private static bool TryReadFields (ref SequenceReader<byte> reader, out VdmVdo vdm) {
            vdm = new VdmVdo ();

            if (!TryReadField (ref reader, out vdm.Tag)) {
                return false;
            }

            if (!(TryReadField (ref reader, out vdm.Total) &&
                    TryReadField (ref reader, out vdm.Num))) {
                //throw new VDMSentenceException ("total or num field is not an integer");
                return false;
            }

            if (!TryReadField (ref reader, out vdm.Sequence)) {
                vdm.Sequence = 0;
            }

            TryReadField (ref reader, out vdm.Channel);
            return true;
        }

        private static bool TryReadField (ref SequenceReader<byte> reader, out ReadOnlySpan<byte> value) {
            return reader.TryReadToAny (out value, fieldSpliter, true);
        }

        private static bool TryReadField (ref SequenceReader<byte> reader, out int value) {
            value = 0;
            if (reader.TryReadToAny (out ReadOnlySpan<byte> sequence, fieldSpliter, true)) {
                var field = Encoding.ASCII.GetString (sequence);
                return int.TryParse (field, out value);
            }
            return false;
        }

        private static bool TryReadField (ref SequenceReader<byte> reader, out string field) {
            field = null;
            if (reader.TryReadToAny (out ReadOnlySpan<byte> sequence, fieldSpliter, true)) {
                field = Encoding.ASCII.GetString (sequence);
                return true;
            }
            return false;
        }

        private static bool TryReadField (ref SequenceReader<byte> reader, out char field) {
            field = '\0';
            if (reader.TryReadToAny (out ReadOnlySpan<byte> sequence, fieldSpliter, true)) {
                if (sequence.Length > 0) {
                    field = (char) sequence[0];
                    return true;
                }
            }
            return false;
        }

        private static bool IsVdmOrVdo (string tag) {
            return tag.EndsWith ("VDM") || tag.EndsWith ("VDO");
        }
    }
}