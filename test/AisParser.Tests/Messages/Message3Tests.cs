using Shouldly;
using Xunit;

namespace AisParser.Tests
{
    public class Message3Tests : MessageTestBase<Message3> {
        [Fact]
        public void Should_parse_message_libais_16 () {
            const string sentence = "!AIVDM,1,1,,B,35MC>W@01EIAn5VA4l`N2;>0015@,0*01";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassAResponseToInterrogation);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (366268061);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (0);
            (message.Sog / 10d).ShouldBe (8.5);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-93.96876833333333d, 0.000001d);
            message.Pos.Latitude.ShouldBe (29.841335d, 0.000001d);
            (message.Cog / 10d).ShouldBe (359.2);
            message.TrueHeading.ShouldBe (359);
            message.UtcSec.ShouldBe (0);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (0x1150);
        }

        [Fact]
        public void Should_parse_message_libais_18 () {
            const string sentence = "!AIVDM,1,1,,A,35NBTh0Oh1G@Dt8EiccBuE3n00nQ,0*05";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassAResponseToInterrogation);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (367305920);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (127);
            (message.Sog / 10d).ShouldBe (0.1);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-122.26239333333334d, 0.000001d);
            message.Pos.Latitude.ShouldBe (38.056821666666664d, 0.000001d);
            (message.Cog / 10d).ShouldBe (75.7);
            message.TrueHeading.ShouldBe (161);
            message.UtcSec.ShouldBe (59);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (3489);
        }

        [Fact]
        public void Should_parse_message_libais_20 () {
            const string sentence = "!AIVDM,1,1,,B,35N0IFP016Jf9rVG8mSB?Acl0Pj0,0*4C";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassAResponseToInterrogation);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (367008090);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (0);
            (message.Sog / 10d).ShouldBe (7);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-73.80338166666667d, 0.000001d);
            message.Pos.Latitude.ShouldBe (40.436715d, 0.000001d);
            (message.Cog / 10d).ShouldBe (57.3);
            message.TrueHeading.ShouldBe (53);
            message.UtcSec.ShouldBe (58);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (134272);
        }
    }
}