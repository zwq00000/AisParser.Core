using System.Buffers;
using System.Text;
using Xunit;

namespace AisParser.Tests {
    public class NmeaTests {
        const string msg = "!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n";
        ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte> (Encoding.ASCII.GetBytes (msg));
        [Fact]
        public void TestCalculateChecksum () {
            var checksum = NmeaSpan.CalculateChecksum (ref sequence);
            Assert.Equal (Nmea.CalculateChecksum (msg), checksum);
        }

        [Fact]
        public void TestGetChecksum () {
            var result = NmeaSpan.CheckChecksum (ref sequence);
            var result1 = Nmea.CheckChecksum (msg);
            Assert.Equal (result1, result);
        }
    }
}