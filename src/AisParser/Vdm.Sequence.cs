using System;
using System.Buffers;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AisParser
{

    /// <summary>
    /// VDM Class
    /// This keeps track partial messages until a complete message has been
    /// received and it holds the sixbit state for exteacting bits from the
    /// message.
    /// </summary>
    public ref struct VdmSequence {

        /// <summary>
        /// !&lt; Message ID 0-31
        /// </summary>
        public int MsgId { get; private set; }

        /// <summary>
        /// VDM message sequence number
        /// </summary>
        public int Sequence { get; private set; }

        /// <summary>
        /// Total # of parts for the message
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Number of the last part stored
        /// </summary>
        public int Num { get; private set; }

        /// <summary>
        ///  AIS Channel character
        /// </summary>
        public char Channel { get; private set; }

        /// <summary>
        ///  sixbit parser state
        /// </summary>
        public SixbitSpan SixState { get; private set; }

        static ReadOnlySpan<byte> NewLine => new byte[] {
            (byte)
            '\r', (byte)
            '\n'
        };

        public VdmStatus Parse(ref SequenceReader<byte> reader){
            if (!reader.TryReadTo (out var line, NewLine)) {
                return VdmStatus.NotAisMessage;
            }
            if (Nmea.CheckChecksum (ref line) != 0) {
                return VdmStatus.ChecksumFailed;
            }
            reader = new SequenceReader<byte> (line);
            Nmea.FindStart (ref reader);
            if (!TryReadField (ref reader, out string tag)) {
                return VdmStatus.NotAisMessage;
            }
            if (!IsVdmOrVdo (tag)) {
                throw new VDMSentenceException ($"{tag} Is Not a VDM or VDO message");
            }
            int total;
            int num;
            int sequence;

            if (!(TryReadField (ref reader, out total) &&
                    TryReadField (ref reader, out num))) {
                //throw new VDMSentenceException ("total or num field is not an integer");
                return VdmStatus.FormatError;
            }

            if (!TryReadField (ref reader, out sequence)) {
                sequence = 0;
            }

            // Are we looking for more message parts?
            if (Total > 0) {
                if (Sequence != sequence || Num != num - 1) {
                    Total = 0;
                    Sequence = 0;
                    Num = 0;
                    return VdmStatus.OutofSequence; // 5
                }

                Num++;
            } else {
                Total = total;
                Num = num;
                Sequence = sequence;
                SixState = new SixbitSpan ();
            }

            if (TryReadField (ref reader, out char channel)) {
                Channel = channel;
            }

            if (TryReadField (ref reader, out ReadOnlySpan<byte> span)) {
                SixState.Add (span);
            } else {
                return VdmStatus.OutofSequence;
            }

            if (total == 0 || Total == num) {
                Total = 0;
                Num = 0;
                Sequence = 0;
                // Get the message id
                try {
                    MsgId = (int) SixState.Get (6);
                } catch (SixbitsExhaustedException) {
                    throw new VDMSentenceException ("Not enough bits for msgid");
                }

                TryReadField (ref reader, out int padbits);
                // Adjust bit count
                SixState.PadBits (padbits);
                // Found a complete packet
                return 0;
            }

            // No complete message yet
            return (VdmStatus) 1;
        }
        public VdmStatus Parse (ref ReadOnlySequence<byte> buffer) {
            var reader = new SequenceReader<byte> (buffer);

            if (!reader.TryReadTo (out var line, NewLine)) {
                return VdmStatus.Incomplete;
            }
            if (Nmea.CheckChecksum (ref line) != 0) {
                return VdmStatus.ChecksumFailed;
            }
            reader = new SequenceReader<byte> (line);
            Nmea.FindStart (ref reader);
            if (!TryReadField (ref reader, out string tag)) {
                return VdmStatus.NotAisMessage;
            }
            if (!IsVdmOrVdo (tag)) {
                throw new VDMSentenceException ($"{tag} Is Not a VDM or VDO message");
            }
            int total;
            int num;
            int sequence;

            if (!(TryReadField (ref reader, out total) &&
                    TryReadField (ref reader, out num))) {
                //throw new VDMSentenceException ("total or num field is not an integer");
                return VdmStatus.FormatError;
            }

            if (!TryReadField (ref reader, out sequence)) {
                sequence = 0;
            }

            // Are we looking for more message parts?
            if (Total > 0) {
                if (Sequence != sequence || Num != num - 1) {
                    Total = 0;
                    Sequence = 0;
                    Num = 0;
                    return VdmStatus.OutofSequence; // 5
                }

                Num++;
            } else {
                Total = total;
                Num = num;
                Sequence = sequence;
                SixState = new SixbitSpan ();
            }

            if (TryReadField (ref reader, out char channel)) {
                Channel = channel;
            }

            if (TryReadField (ref reader, out ReadOnlySpan<byte> span)) {
                SixState.Add (span);
            } else {
                return VdmStatus.OutofSequence;
            }

            if (total == 0 || Total == num) {
                Total = 0;
                Num = 0;
                Sequence = 0;
                // Get the message id
                try {
                    MsgId = (int) SixState.Get (6);
                } catch (SixbitsExhaustedException) {
                    throw new VDMSentenceException ("Not enough bits for msgid");
                }

                TryReadField (ref reader, out int padbits);
                // Adjust bit count
                SixState.PadBits (padbits);
                // Found a complete packet
                return 0;
            }

            // No complete message yet
            return (VdmStatus) 1;
        }

        private readonly static byte[] fieldSpliter = new byte[] {
            (byte)
            ',', (byte)
            '*'
        };

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

        private static bool TryReadFields (ref SequenceReader<byte> reader, out string[] fields, int length) {
            fields = new string[length];
            for (int i = 0; i < length - 1; i++) {
                if (!reader.TryReadToAny (out ReadOnlySpan<byte> sequence, fieldSpliter)) {
                    return false;
                }
                fields[i] = Encoding.ASCII.GetString (sequence);
            }
            fields[length - 1] = Encoding.ASCII.GetString (reader.UnreadSpan);
            return true;
        }

        private static bool IsVdmOrVdo (ReadOnlySpan<byte> tag) {
            var vdm = Encoding.ASCII.GetBytes ("VDM");
            var vdo = Encoding.ASCII.GetBytes ("VDO");
            return tag.EndsWith (vdm) || tag.EndsWith (vdo);
        }

        private static bool IsVdmOrVdo (string tag) {
            return tag.EndsWith ("VDM") || tag.EndsWith ("VDO");
        }

    }

    public struct VdmVdo {
        // public VdmSequence () {
        //     Total = 0;
        //     Sequence = 0;
        //     Num = 0;
        // }

        /// <summary>
        /// Message ID 0-31
        /// </summary>
        public int MsgId;

        /// <summary>
        /// VDM message sequence number
        /// </summary>
        public int Sequence;

        /// <summary>
        /// Total # of parts for the message
        /// </summary>
        public int Total;

        /// <summary>
        /// Number of the last part stored
        /// </summary>
        public int Num;

        /// <summary>
        ///  AIS Channel character
        /// </summary>
        public string Channel;

        /// <summary>
        /// vdm/vdo tag
        /// </summary>
        internal string Tag;
    }
}