using Shouldly;
using Xunit;

namespace AisParser.Tests
{
    public class Message2Tests : MessageTestBase<Message2> {
        [Fact]
        public void Should_parse_message () {
            const string sentence = "!AIVDM,1,1,,B,25Mw@DP000qR9bFA:6KI0AV@00S3,0*0A";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassAAssignedSchedule);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (366989394);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (0);
            (message.Sog / 10d).ShouldBe (0);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.High);
            message.Pos.Longitude.ShouldBe (-90.40670166666666d, 0.000001d);
            message.Pos.Latitude.ShouldBe (29.985461666666666d, 0.000001d);
            (message.Cog / 10d).ShouldBe (230.5);
            message.TrueHeading.ShouldBe (51);
            message.UtcSec.ShouldBe (8);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (0x8C3);
        }
    }
}