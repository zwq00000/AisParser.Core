using Xunit;

namespace AisParser.Tests
{
    public class SixbitTests{

        [Fact]
        public void Test1(){
            var bits = "19NS7Sp02wo?HETKA2K6mUM20<L=";
            var sixbit = new Sixbit(bits);
            Assert.Equal(28, sixbit.Length);
            Assert.Equal(168,sixbit.BitLength);

            Assert.Equal("AI",sixbit.GetString(2));
        }
    }
}