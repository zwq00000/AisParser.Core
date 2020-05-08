using System;
using System.Buffers;
using System.Text;
using Xunit;

namespace AisParser.Tests {
    public class NmeaTests {
        const string msg = "!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n";
        ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte> (Encoding.ASCII.GetBytes (msg));
        [Fact]
        public void TestCalculateChecksum () {
            var checksum = Nmea.CalculateChecksum (ref sequence);
            Assert.Equal (Nmea.CalculateChecksum (msg), checksum);
        }

        [Fact]
        public void TestGetChecksum () {
            var result = Nmea.CheckChecksum (ref sequence);
            var result1 = Nmea.CheckChecksum (msg);
            Assert.Equal (result1, result);
        }

        [Fact]
        public void TestSplit () {
            var reader = new SequenceReader<byte> (sequence);
            var newLine = Encoding.ASCII.GetBytes ("\r\n");
            Assert.True (reader.TryReadTo (out var line, newLine));

            var count = 0;
            reader = new SequenceReader<byte> (line);
            while (reader.TryReadTo (out ReadOnlySpan<byte> part, (byte)
                    ',', true)) {
                //Assert.Empty(part);
                Console.WriteLine (Encoding.ASCII.GetString (part));
                count++;
            }
            Assert.Equal (6, count);
        }

        [Fact]
        public void TestReadPayload () {
            var reader = new SequenceReader<byte> (sequence);
            var newLine = Encoding.ASCII.GetBytes ("\r\n");
            Assert.True (reader.TryReadTo (out var line, newLine));
            Assert.True (Nmea.TryReadPayload (ref line, out var payload));
            Assert.Equal ("19NS7Sp02wo?HETKA2K6mUM20<L=", Encoding.ASCII.GetString (payload));
        }
    }
}