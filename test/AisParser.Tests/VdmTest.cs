using System.Text;
using System.Buffers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;

namespace AisParser.Tests
{
    public class VdmParserTest : TestBase {

        public VdmParserTest () {
            
        }

        [Fact]
        public void TestMsgid () {
            const string message = "!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n";
            var sequence = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(message));
            var result = VdmParser.TryParse(ref sequence,out var vdm ,out var sixbit);
            Assert.Equal(true,result);
            Assert.Equal(1,vdm.MsgId);

            var msg = (Message1)vdm.Build(ref sixbit);
            Assert.NotNull(msg);

            AssertEquals("msgid", 1, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 636012431, msg.UserId);
            AssertEquals("nav_status", 8, msg.NavStatus);
            AssertEquals("rot", 0, msg.Rot);
            AssertEquals("sog", 191, msg.Sog);
            AssertEquals("pos_acc", 1, msg.PosAcc);
            //AssertEquals("longitude", -73481550, msg.Pos.Longitude);
            //AssertEquals("latitude", 28590700, msg.Pos.Latitude);
            AssertEquals("cog", 1750, msg.Cog);
            AssertEquals("true_heading", 174, msg.TrueHeading);
            AssertEquals("utc_sec", 33, msg.UtcSec);
            AssertEquals("regional", 0, msg.Regional);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("sync_state", 0, msg.SyncState);
            AssertEquals("slot_timeout", 3, msg.SlotTimeout);
            AssertEquals("sub_message", 1805, msg.SubMessage);
        }

        [Fact]
        public void TestAdd () {
            // var result = _vdmMessage.Add ("!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n");
            // AssertEquals ("Adding message 1 failed", 0, (int) result);

            // _vdmMessage = new VdmSequence ();
            // result = _vdmMessage.Add ("!AIVDM,2,1,6,B,55ArUT02:nkG<I8GB20nuJ0p5HTu>0hT9860TV16000006420BDi@E53,0*33\r\n");
            // AssertEquals ("Adding message 5 part 1 failed", 1, (int) result);

            // result = _vdmMessage.Add ("!AIVDM,2,2,6,B,1KUDhH888888880,2*6A");
            // AssertEquals ("Adding message 5 part 1 failed", 0, (int) result);

        }

    }
}