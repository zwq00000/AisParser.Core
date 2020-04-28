using Xunit;

namespace AisParser.Tests {
    public class MessageTestBase<TMsg> where TMsg : Messages {
        private Vdm vdm = new Vdm ();
        public TMsg Parse (string body) {
            var status = vdm.Add (body);
            Assert.Equal (status, VdmStatus.Complete);
            return (TMsg) vdm.ToMessage ();
        }
    }
}