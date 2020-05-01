using System;
using System.Buffers;
using Xunit;

namespace AisParser.Tests {
    public class SixbitTests {

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
            var bits = "19NS7Sp02wo?HETKA2K6mUM20<L=";
            Parse (bits);
        }

        private void Parse (ReadOnlySpan<char> span) {

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