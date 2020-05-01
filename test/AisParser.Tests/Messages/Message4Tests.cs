using Shouldly;
using Xunit;

namespace AisParser.Tests
{

    public class Message4Tests : MessageTestBase<Message4> {

        [Fact]
        public void Should_parse_message () {
            const string sentence = "!AIVDM,1,1,,A,402MN7iv:HFssOrrk4M4EVw02L1T,0*29";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.BaseStationReport);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (2579999);
            message.UtcYear.ShouldBe (2018);
            message.UtcMonth.ShouldBe (9);
            message.UtcDay.ShouldBe (16);
            message.UtcHour.ShouldBe (22);
            message.UtcMinute.ShouldBe (59);
            message.UtcSecond.ShouldBe (59);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-1.110023d, 0.000001d);
            message.Pos.Latitude.ShouldBe (50.799618d, 0.000001d);
            message.PosType.ShouldBe ((int) PositionFixType.InternalGNSS);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.InUse);
            message.SyncState.ShouldBe (0);
            message.SlotTimeout.ShouldBe (7);
            message.SubMessage.ShouldBe (100);
            //message.SubMessage.ShouldBe(114788);
        }

        [Fact]
        public void Should_parse_another_message () {
            const string sentence = "!AIVDM,1,1,,B,403OK@Quw35W<rsg:hH:wK70087D,0*6E";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.BaseStationReport);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (3660610);
            message.UtcYear.ShouldBe (2015);
            message.UtcMonth.ShouldBe (12);
            message.UtcDay.ShouldBe (6);
            message.UtcHour.ShouldBe (5);
            message.UtcMinute.ShouldBe (39);
            message.UtcSecond.ShouldBe (12);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.High);
            message.Pos.Longitude.ShouldBe (-70.83633333333334d, 0.000001d);
            message.Pos.Latitude.ShouldBe (42.24316666666667d, 0.000001d);
            message.PosType.ShouldBe ((int) PositionFixType.Surveyed);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe(33236);
        }

        [Fact]
        public void Should_parse_message_libais_25 () {
            const string sentence = "!AIVDM,1,1,,A,402u=TiuaA000r5UJ`H4`?7000S:,0*75";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.BaseStationReport);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (3100051);
            message.UtcYear.ShouldBe (2010);
            message.UtcMonth.ShouldBe (5);
            message.UtcDay.ShouldBe (2);
            message.UtcHour.ShouldBe (0);
            message.UtcMinute.ShouldBe (0);
            message.UtcSecond.ShouldBe (0);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.High);
            message.Pos.Longitude.ShouldBe (-82.6661d, 0.000001d);
            message.Pos.Latitude.ShouldBe (42.069433333333336d, 0.000001d);
            message.PosType.ShouldBe ((int) PositionFixType.Surveyed);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe(2250);
        }

        [Fact]
        public void Should_parse_message_libais_26 () {
            const string sentence = "!AIVDM,1,1,,A,403OweAuaAGssGWDABBdKBA006sd,0*07";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.BaseStationReport);
            message.Repeat.ShouldBe (0);
            message.UserId.ShouldBe (3669941);
            message.UtcYear.ShouldBe (2010);
            message.UtcMonth.ShouldBe (5);
            message.UtcDay.ShouldBe (2);
            message.UtcHour.ShouldBe (23);
            message.UtcMinute.ShouldBe (59);
            message.UtcSecond.ShouldBe (59);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            message.Pos.Longitude.ShouldBe (-117.24025166666667d, 0.000001d);
            message.Pos.Latitude.ShouldBe (32.670415d, 0.000001d);
            message.PosType.ShouldBe ((int) PositionFixType.Gps);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe(28396);
        }

        [Fact]
        public void Should_parse_message_libais_27 () {
            const string sentence = "!AIVDM,1,1,,B,4h3OvjAuaAGsro=cf0Knevo00`S8,0*7E";

            var message = Parse (sentence);
            message.ShouldNotBeNull ();
            message.MsgId.ShouldBe ((int) AisMessageType.BaseStationReport);
            message.Repeat.ShouldBe (3);
            message.UserId.ShouldBe (3669705);
            message.UtcYear.ShouldBe (2010);
            message.UtcMonth.ShouldBe (5);
            message.UtcDay.ShouldBe (2);
            message.UtcHour.ShouldBe (23);
            message.UtcMinute.ShouldBe (59);
            message.UtcSecond.ShouldBe (58);
            message.PosAcc.ShouldBe ((int) PositionAccuracy.High);
            message.Pos.Longitude.ShouldBe (-122.84d, 0.000001d);
            message.Pos.Latitude.ShouldBe (48.68009833333333d, 0.000001d);
            message.PosType.ShouldBe ((int) PositionFixType.Surveyed);
            message.Spare.ShouldBe (0);
            message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe(166088);
        }

        [Fact]
        public void Should_parse_message_20190212_154105 () {
            const string sentence = "!AIVDM,1,1,,A,402MN7iv<V5r,0*16";

            Assert.Throws<AisMessageException> (() => Parse (sentence));
            // message.ShouldNotBeNull ();
            // message.MsgId.ShouldBe ((int) AisMessageType.BaseStationReport);
            // message.Repeat.ShouldBe (0);
            // message.UserId.ShouldBe (2579999);
            // message.UtcYear.ShouldBe (2019);
            // message.UtcMonth.ShouldBe (2);
            // message.UtcDay.ShouldBe (12);
            // message.UtcHour.ShouldBe (5);
            // message.UtcMinute.ShouldBe (58);
            // message.UtcSecond.ShouldBe (0);
            // message.PosAcc.ShouldBe ((int) PositionAccuracy.Low);
            // message.Pos.Longitude.ShouldBe (0d, 0.000001d);
            // message.Pos.Latitude.ShouldBe (0d, 0.000001d);
            // message.PosType.ShouldBe ((int) PositionFixType.Undefined1);
            // message.Spare.ShouldBe (0);
            // message.Raim.ShouldBe ((int) Raim.NotInUse);
            //message.RadioStatus.ShouldBe(0);
        }
    }
}