using System;
using Shouldly;
using Xunit;

namespace AisParser.Tests {

    public class Message1Tests : MessageTestBase<Message1> {
        [Fact]
        public void Should_parse_message () {
            const string sentence = "!AIVDM,1,1,,B,13GmFd002pwrel@LpMu8L6qn8Vp0,0*56";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassA);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (226318000);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (0);
            (message.Sog / 10d).ShouldBe (18.4);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.High);
            message.Pos.Longitude.ShouldBe (-1.154333d, 0.000001d);
            message.Pos.Latitude.ShouldBe (50.475500d, 0.000001d);
            (message.Cog / 10d).ShouldBe (216);
            message.TrueHeading.ShouldBe (220);
            message.UtcSec.ShouldBe (59);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (2);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (0x26E00);
        }

        [Fact]
        public void Should_parse_message_libais_4 () {
            const string sentence = "!AIVDM,1,1,,A,15B4FT5000JRP>PE6E68Nbkl0PS5,0*70";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassA);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (354490000);
            message.NavStatus.ShouldBe ((int) NavigationStatus.Moored);
            message.Rot.ShouldBe (0);
            (message.Sog / 10d).ShouldBe (0);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-76.34866666666667d, 0.000001d);
            message.Pos.Latitude.ShouldBe (36.873d, 0.000001d);
            (message.Cog / 10d).ShouldBe (217);
            message.TrueHeading.ShouldBe (345);
            message.UtcSec.ShouldBe (58);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (133317);
        }

        [Fact]
        public void Should_parse_message_libais_6 () {
            const string sentence = "!AIVDM,1,1,,B,15Mw1U?P00qNGTP@v`0@9wwn26sd,0*0E";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassA);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (366985620);
            message.NavStatus.ShouldBe ((int) NavigationStatus.NotDefined);
            message.Rot.ShouldBe (128);
            (message.Sog / 10d).ShouldBe (0);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.High);
            message.Pos.Longitude.ShouldBe (-91.23304d, 0.000001d);
            message.Pos.Latitude.ShouldBe (29.672108333333334d, 0.000001d);
            (message.Cog / 10d).ShouldBe (3.9);
            message.TrueHeading.ShouldBe (511);
            message.UtcSec.ShouldBe (59);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.InUse);
            //message.RadioStatus.ShouldBe (28396);
        }

        [Fact]
        public void Should_parse_message_libais_8 () {
            const string sentence = "!AIVDM,1,1,,B,15N5s90P00IB>dtA7f<pOwv00<1a,0*2B";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassA);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (367098660);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (128);
            (message.Sog / 10d).ShouldBe (0);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-93.88475d, 0.000001d);
            message.Pos.Latitude.ShouldBe (29.920511666666666d, 0.000001d);
            (message.Cog / 10d).ShouldBe (217.5);
            message.TrueHeading.ShouldBe (511);
            message.UtcSec.ShouldBe (0);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (49257);
        }

        [Fact]
        public void Should_parse_message_libais_10 () {
            const string sentence = "!AIVDM,1,1,,B,15Mq4J0P01EREODRv4@74gv00HRq,0*72";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassA);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (366888040);
            message.NavStatus.ShouldBe ((int) NavigationStatus.UnderWayUsingEngine);
            message.Rot.ShouldBe (128);
            (message.Sog / 10d).ShouldBe (0.1);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-146.29038333333332d, 0.000001d);
            message.Pos.Latitude.ShouldBe (61.114133333333335d, 0.000001d);
            (message.Cog / 10d).ShouldBe (181);
            message.TrueHeading.ShouldBe (511);
            message.UtcSec.ShouldBe (0);
            message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe (100537);
        }

        [Fact]
        public void Should_parse_message_with_type_0 () {
            const string sentence = "!AIVDM,1,1,,B,001vUEEEOP@p2mLWh0nWvd107@jc,0*15";

            var message = Assert.Throws<InvalidCastException>(()=> Parse (sentence));
            // message.ShouldNotBeNull ();
            // message.MsgId.ShouldBe ((int) AisMessageType.PositionReportClassA);
            // message.Repeat.ShouldBe (0);
            // message.UserId.ShouldBe (2073941);
            // message.NavStatus.ShouldBe ((int) NavigationStatus.Moored);
            // message.Rot.ShouldBe (85); // TODO: should this be 322.5 ?
            // (message.Sog / 10d).ShouldBe (99.2);
            // message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            // message.Pos.Longitude.ShouldBe (-211.4531500d, 0.000001d); // TODO: check longitude value
            // message.Pos.Latitude.ShouldBe (69.4685233d, 0.000001d);
            // (message.Cog / 10d).ShouldBe (204.2);
            // message.TrueHeading.ShouldBe (384);
            // message.UtcSec.ShouldBe (32);
            // message.Regional.ShouldBe ((int) ManeuverIndicator.NotAvailable);
            // message.Spare.ShouldBe (1);
            // message.Raim.ShouldBe ((int) Raim.InUse);
            //message.RadioStatus.ShouldBe (330923);
        }
    }
}