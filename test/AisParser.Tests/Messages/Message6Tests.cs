using Shouldly;
using Xunit;

namespace AisParser.Tests
{
    public class Message6Tests:MessageTestBase<Message6>{

         [Fact]
        public void Should_parse_message()
        {
            const string sentence = "!AIVDM,1,1,,A,6>h8nIT00000>d`vP000@00,2*53";

            var message = Parse(sentence);
            message.ShouldNotBeNull();
            message.MsgId.ShouldBe((int)AisMessageType.BinaryAddressedMessage);
            message.Repeat.ShouldBe(0);
            message.UserId.ShouldBe(990000742);
            message.Sequence.ShouldBe(1);
            message.Destination.ShouldBe(0);
            message.Retransmit.ShouldBeFalse();
            //message.DesignatedAreaCode.ShouldBe(235);
            //message.FunctionalId.ShouldBe(10);
            //message.Data.ShouldBe("O(D");
        }
    }
}