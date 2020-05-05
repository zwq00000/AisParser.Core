using System;
using System.Buffers;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AisParser {

    /// <summary>
    ///     NMEA message class
    ///     Provides methods for finding the start of a NMEA message,
    ///     and calculating the checksum of the message.
    /// </summary>
    public partial class NmeaSpan {
        //    private int _checksum;
        //     private string _msg;

        //     /// <summary>
        //     ///     Initialize it with a NMEA message string
        //     /// </summary>
        //     /// <param name="msg"></param>
        //     public void Init (ReadOnlySpan<char> msg) {
        //         _msg = msg;
        //     }

        //     #region Static methods

        //     private static byte[] fieldSplter = new byte[]{(byte)',',(byte)'*'};

        //     /// <summary>
        //     /// find next Nmea filed
        //     /// </summary>
        //     /// <param name="msg"></param>
        //     /// <param name="offset">field start pos</param>
        //     /// <returns></returns>
        //     public static string TryNextField (in ReadOnlySequence<byte> msg, int offset = 0) {
        //         var reader = new SequenceReader<byte>(msg);
        //         if(reader.TryAdvanceToAny(fieldSplter)){
        //             return 
        //         }
        //         for (var i = offset; i < msg.Length; i++) {
        //             var c = msg[i];
        //             if (c == ',' || c == '*') {
        //                 return msg.Substring (offset, i - offset);
        //             }
        //         }

        //         return null;
        //     }

        //     /// <summary>
        //     ///     Find the start of a NMEA sentence '!' or '$' character
        //     ///     Returns:
        //     ///     - index of the start character
        //     ///     The NMEA sentence may not always start at the beginning of the buffer,
        //     ///     use this routine to make sure the start of the sentence has been found.
        //     /// </summary>
        //     /// <returns>index of the start character</returns>
        //     /// <exception cref="StartNotFoundException"></exception>
        //     public static int FindStart (string msg) {
        //         var i = 0;
        //         foreach (var x in msg) {
        //             if (x == '!' || x == '$') return i;
        //             i++;
        //         }

        //         throw new StartNotFoundException ("NMEA Start Not Found");
        //     }

        /// <summary>
        ///     Calculate the checksum of a NMEA 0183 sentence
        ///     This method will calculate the checksum of the string by skipping the
        ///     ! or $ character and stopping at the * character or end of string.
        ///     The checksum is stored in this.checksum
        /// </summary>
        /// <exception cref="StartNotFoundException"></exception>
        /// <exception cref="IllegalNmeaCharacterException"></exception>
        public static int CalculateChecksum (ref ReadOnlySequence<byte> msg) {
            var reader = new SequenceReader<byte> (msg);
            if (TryCalculateChecksum (ref reader, out var checksum)) {
                return checksum;
            }
            throw new IllegalNmeaCharacterException ("Start Character Found before Checksum");
        }

        static bool TryCalculateChecksum (ref SequenceReader<byte> reader, out int checksum) {
            checksum = 0;
            if (reader.TryAdvanceToAny (startChar, true)) {
                if (reader.TryReadTo (out ReadOnlySpan<byte> span, (byte)
                        '*')) {
                    checksum = span.XOR ();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///     Check and return the checksum of a NMEA 0183 sentence
        ///     This method calls calculateChecksum() and updates this.checksum
        /// </summary>
        /// <returns>
        ///     - 0 if it does match
        ///     - 1 if it does not match
        ///     - 2 if there was an error
        /// </returns>
        public static int CheckChecksum (ref ReadOnlySequence<byte> msg) {
            string msgChecksum;
            var reader = new SequenceReader<byte> (msg);
            if (TryCalculateChecksum (ref reader, out var checksum)) {
                msgChecksum = Encoding.ASCII.GetString (reader.UnreadSpan.Slice (0, 2));
                if (int.TryParse (msgChecksum,
                        System.Globalization.NumberStyles.HexNumber,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var value)) {
                    return checksum == value?0 : 1;
                }
            }
            return 2;
        }

        //     #endregion

        static readonly byte[] startChar = new byte[] {
            (byte)
            '!', (byte)
            '$'
        };
        /// <summary>
        ///     Find the start of a NMEA sentence '!' or '$' character
        ///     Returns:
        ///     - index of the start character
        ///     The NMEA sentence may not always start at the beginning of the buffer,
        ///     use this routine to make sure the start of the sentence has been found.
        /// </summary>
        /// <returns>index of the start character</returns>
        /// <exception cref="StartNotFoundException"></exception>
        public int FindStart (ref SequenceReader<byte> reader) {

            if (reader.TryAdvanceToAny (startChar, true)) {
                return (int) reader.Consumed;
            }
            // var i = 0;
            // foreach (var x in _msg) {
            //     if (x == '!' || x == '$') return i;
            //     i++;
            // }

            throw new StartNotFoundException ("NMEA Start Not Found");
        }

        //     /// <summary>
        //     ///     Calculate the checksum of a NMEA 0183 sentence
        //     ///     This method will calculate the checksum of the string by skipping the
        //     ///     ! or $ character and stopping at the * character or end of string.
        //     ///     The checksum is stored in this.checksum
        //     /// </summary>
        //     /// <exception cref="StartNotFoundException"></exception>
        //     /// <exception cref="IllegalNmeaCharacterException"></exception>
        //     public void CalculateChecksum () {
        //         // Find start of sentence, after a '!' or '$'
        //         var ptr = FindStart () + 1;

        //         _checksum = 0;
        //         foreach (var c in _msg.Skip (ptr)) {
        //             if (c == '!' || c == '$') {
        //                 throw new IllegalNmeaCharacterException ("Start Character Found before Checksum");
        //             }

        //             if (c == '*') break;

        //             _checksum ^= c;
        //         }
        //     }

        //     /// <summary>
        //     ///     Check and return the checksum of a NMEA 0183 sentence
        //     ///     This method calls calculateChecksum() and updates this.checksum
        //     /// </summary>
        //     /// <returns>
        //     ///     - 0 if it does match
        //     ///     - 1 if it does not match
        //     ///     - 2 if there was an error
        //     /// </returns>
        //     public int CheckChecksum () {
        //         string msgChecksum;

        //         try {
        //             CalculateChecksum ();
        //         } catch (StartNotFoundException) {
        //             return 2;
        //         } catch (IllegalNmeaCharacterException) {
        //             return 2;
        //         }

        //         var ptr = _msg.IndexOf ('*');
        //         if (ptr < 0) return 2;

        //         try {
        //             msgChecksum = _msg.Substring (ptr + 1, ptr + 3 - (ptr + 1));
        //         } catch (IndexOutOfRangeException) {
        //             return 2;
        //         }

        //         if (Convert.ToInt32 (msgChecksum, 16) != _checksum) return 1;
        //         return 0;
        //     }
    }
}