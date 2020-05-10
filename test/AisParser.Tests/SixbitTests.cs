using System;
using System.Buffers;
using System.Collections;
using System.Text;
using Xunit;

namespace AisParser.Tests {
    public class SixbitTests {

        const string payload = "19NS7Sp02wo?HETKA2K6mUM20<L=";
        ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte> (Encoding.ASCII.GetBytes (payload));

        [Fact]
        public void Test1 () {
            var bits = "19NS7Sp02wo?HETKA2K6mUM20<L=";
            var sixbit = new Sixbit (bits);
            Assert.Equal (28, sixbit.Length);
            Assert.Equal (168, sixbit.BitLength);

            Assert.Equal ("AI", sixbit.GetString (2));
        }

        [Fact]
        public void TestSpan () {
            var msgId = new Sixbit ("13GmFd002pwrel@LpMu8L6qn8Vp0").Get(6);
            Assert.Equal(1,msgId);
            //Assert.Equal (1, msgId);
            Assert.Equal ('1', (char) Read (sequence.ToArray (), 0, 6));
            Assert.Equal('1', Read(ref sequence,0,6));

            Assert.Equal(1, Read(ref sequence,6,2));

            var bits = new BitArray (new bool[1]);

        }
        private static readonly int[] _pow2Mask = { 0x00, 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F };

        private long Read (ref ReadOnlySequence<byte> sixbits, int start, int count) {
            var startPos = sixbits.GetPosition (start / 6);
            if (sixbits.TryGet (ref startPos, out var buf, false)) {
                var offset = start % 6;
                var len = (int) Math.Round (count / 6f);
                var span = buf.Span;
                long result = 0;
                result = span[0] & _pow2Mask[6 - offset];
                for (int i = 1; i < len; i++) {
                    result += (result << 6) + Binfrom6Bit (span[i]);
                }
                return result;
            }
            return 0;
        }

        private long Read (ReadOnlySpan<byte> sixbits, int start, int count) {
            var index = start / 6;
            var offset = start % 6;
            var len = Math.Round (count / 6f);
            //sixbits.AsSpan().Slice(index,len);
            long result = 0;
            result = sixbits[index] & _pow2Mask[6 - offset];
            for (int i = 1; i < len; i++) {
                result += (result << 6) + Binfrom6Bit (sixbits[index + i]);
            }
            return result;
        }
        private int GetInt (ref SequenceReader<byte> reader) {
            var bytes = new byte[6];
            if (reader.TryCopyTo (bytes)) {
                int result = 0;
                for (int i = 0; i < 6; i++) {
                    result += result << 6 + Binfrom6Bit (bytes[i]);
                }
                return result;
            }
            return 0;
        }

        /// <summary>
        ///     Convert an ASCII value to a 6-bit binary value
        ///     This function checks the ASCII value to make sure it can be converted.
        ///     If not, it throws an IllegalArgumentException
        ///     Otherwise it returns the 6-bit binary value.
        /// </summary>
        /// <param name="ascii">
        ///     character to convert
        ///     This is used to convert the packed 6-bit value to a binary value. It
        ///     is not used to convert data from fields such as the name and
        ///     destination -- Use ais2ascii() instead.
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        private static int Binfrom6Bit (int ascii) {
            if (ascii < 0x30 || ascii > 0x77 || ascii > 0x57 && ascii < 0x60)
                throw new ArgumentException ("Illegal 6-bit ASCII value");
            if (ascii < 0x60)
                return (ascii - 0x30) & 0x3F;
            return (ascii - 0x38) & 0x3F;
        }

        // private void Parse (ReadOnlySequence<byte> buffer,int len) {
        //     var reader = new SequenceReader<byte> (buffer);

        //     if(reader.TryRead(out byte b)){
        //         Binfrom6Bit(b);
        //     }
        // }

        // public void TestAppend () {

        // }
    }
}