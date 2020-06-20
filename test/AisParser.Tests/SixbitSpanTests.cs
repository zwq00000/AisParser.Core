using System.Text;
using Xunit;
/**
*! \file
\brief 6-bit packed ASCII functions
\author Copyright 2006-2008 by Brian C. Lane <bcl@brianlane.com>, All Rights Reserved
\version 1.8
*/
namespace AisParser.Tests
{
    public class SixbitSpanTests : TestBase {

        [Fact]
        public void TestMessage1 () {
            const string sentence = "19NS7Sp02wo?HETKA2K6mUM20<L=";
            var sixbit = new Sixbit (sentence);
            sixbit.Get (6);
            var msg = new Message1 (sixbit);
            AssertMessage1(msg);

            var bytes = Encoding.ASCII.GetBytes (sentence);
            var sixState = new SixbitSpan (bytes);
            Assert.Equal (168, sixState.Length);
            var msgId = sixState.Get (6);
            var rep = sixState.Get (2);
            var mmsi = sixState.Get (30);
            var NavStatus = (int) sixState.Get (4);
            var Rot = (int) sixState.Get (8);
            var Sog = (int) sixState.Get (10);
            var PosAcc = (int) sixState.Get (1);
            var longitude = sixState.Get (28);
            var latitude = sixState.Get (27);
            var Cog = (int) sixState.Get (12);
            var TrueHeading = (int) sixState.Get (9);
            var UtcSec = (int) sixState.Get (6);
            var Regional = (int) sixState.Get (2);
            var Spare = (int) sixState.Get (3);
            var Raim = (int) sixState.Get (1);
            var SyncState = (int) sixState.Get (2);
            //Assert.Equal (1, msgId);

            Assert.Equal (1, msgId);
            Assert.Equal (0, rep);
            //Assert.Equal (226318000, mmsi);

            Assert.Equal (msg.MsgId, msgId);
            Assert.Equal (msg.Repeat, rep);
            Assert.Equal (msg.UserId, mmsi);
            Assert.Equal (msg.NavStatus, NavStatus);
            Assert.Equal (msg.Rot, Rot);
            Assert.Equal (msg.Sog, Sog);
            Assert.Equal (msg.PosAcc, PosAcc);
            //Assert.Equal (msg1.Pos.Longitude, longitude);
            //Assert.Equal (msg1.Pos.Latitude, latitude);
            Assert.Equal (msg.Cog, Cog);
            Assert.Equal (msg.TrueHeading, TrueHeading);
            Assert.Equal (msg.UtcSec, UtcSec);
            Assert.Equal (msg.Regional, Regional);
            Assert.Equal (msg.Spare, Spare);
            Assert.Equal (msg.Raim, Raim);
            Assert.Equal (msg.SyncState, SyncState);
            AssertMessage1 (msg);
        }

        private void AssertMessage1 (Message1 msg) {
            AssertEquals ("msgid", 1, msg.MsgId);
            AssertEquals ("repeat", 0, msg.Repeat);
            AssertEquals ("userid", 636012431, msg.UserId);
            AssertEquals ("nav_status", 8, msg.NavStatus);
            AssertEquals ("rot", 0, msg.Rot);
            AssertEquals ("sog", 191, msg.Sog);
            AssertEquals ("pos_acc", 1, msg.PosAcc);
            //AssertEquals("longitude", -73481550, msg.Pos.Longitude);
            //AssertEquals("latitude", 28590700, msg.Pos.Latitude);
            AssertEquals ("cog", 1750, msg.Cog);
            AssertEquals ("true_heading", 174, msg.TrueHeading);
            AssertEquals ("utc_sec", 33, msg.UtcSec);
            AssertEquals ("regional", 0, msg.Regional);
            AssertEquals ("spare", 0, msg.Spare);
            AssertEquals ("raim", 0, msg.Raim);
            AssertEquals ("sync_state", 0, msg.SyncState);
            AssertEquals ("slot_timeout", 3, msg.SlotTimeout);
            AssertEquals ("sub_message", 1805, msg.SubMessage);

        }

        [Fact]
        public void TestMessageBuilder () {
            const string sentence = "19NS7Sp02wo?HETKA2K6mUM20<L=";
            var bytes = Encoding.ASCII.GetBytes (sentence);
            var sixState = new SixbitSpan (bytes);
            var msgId = (int)sixState.Get(6);
            var vdm = new VdmVdo(){MsgId=msgId};
            var msg = (Message1) vdm.Build(ref sixState);
            AssertMessage1(msg);
        }
    }
}