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
    public partial class Nmea {
        static readonly byte[] StartChars = new byte[] {
            (byte)
            '!', (byte)
            '$'
        };
        const byte FieldSpliter = (byte)
        ',';

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
            if (reader.TryAdvanceToAny (StartChars, true)) {
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

        /// <summary>
        ///     Find the start of a NMEA sentence '!' or '$' character
        ///     Returns:
        ///     - index of the start character
        ///     The NMEA sentence may not always start at the beginning of the buffer,
        ///     use this routine to make sure the start of the sentence has been found.
        /// </summary>
        /// <returns>index of the start character</returns>
        /// <exception cref="StartNotFoundException"></exception>
        public static int FindStart (ref SequenceReader<byte> reader) {
            if (reader.TryAdvanceToAny (StartChars, true)) {
                return (int) reader.Consumed;
            }
            throw new StartNotFoundException ("NMEA Start Not Found");
        }

        public static bool TryGetNextField (ref SequenceReader<byte> reader, out ReadOnlySpan<byte> field) {
            return reader.TryReadTo (out field, FieldSpliter);
        }

        /// <summary>
        /// get AIS sentence Payload (Field 5)
        /// 从技术上讲，NMEA0183实际上并不需要以A开头的句子为AIS。此格式可用于任何封装的数据。字段1-4的语法和语义是固定的，并且填充位字段和NEA校验和是必需的，但是有效负载字段可以包含任何封装的数据。
        /// 但是，可以肯定的是，任何在字段5中包含A或B信道代码的句子都是AIVDM/AIVDO。
        /// </summary>
        /// <remark>
        /// 字段1，！AIVDM将其标识为AIVDM数据包。
        /// 字段2（在此示例中为1）是当前正在累积的消息中的片段计数。每个句子的有效负载大小受NMEA 0183的最大82个字符限制，因此有时需要将有效负载划分为几个片段句子。
        /// 字段3（在此示例中为1）是此句子的片段编号。这将是一个基础。片段数为1且片段数为1的句子本身就是完整的。
        /// 字段4（在此示例中为空）是多句消息的顺序消息ID。
        /// 字段5（在此示例中为B）是无线电信道代码。AIS使用来自两个VHF无线电信道的双工的高端：AIS信道A为161.975Mhz（87B）；AIS通道B为162.025Mhz（88B）。在野外，也可能会遇到频道代码'1'和'2'。这些标准没有规定对这些标准的解释，但是很明显。
        /// 字段6（在此示例中为177KQJ5000G？tO`K> RA1wUbN0TKH）是数据有效负载。我们将在后面的部分中描述如何对此进行解码。
        /// 字段7（0）是填充数据有效载荷到6位边界（从0到5）所需的填充位数。等效地，从中减去5可以知道在最后6位半字节中有多少个最低有效位数据有效载荷应被忽略。
        /// 请注意，该填充字节与 [ITU-1371] 空中AIS消息中字节对齐的要求存在棘手的交互；请参阅后面的部分中有关消息长度和对齐方式的详细讨论。
        /// </remark>
        /// <param name="line"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static bool TryReadPayload (ref ReadOnlySequence<byte> line, out ReadOnlySpan<byte> payload) {
            var reader = new SequenceReader<byte> (line);
            
            for (int i = 0; i < 5; i++) {
                //skip 1~5 field
               if(!reader.TryAdvanceTo (FieldSpliter)){
                   payload = null;
                   return false;
               }
            }
            return reader.TryReadTo (out payload, FieldSpliter);
        }

    }
}