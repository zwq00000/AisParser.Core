namespace AisParser {
    /// <summary>
    ///     AIS Itdma class
    /// </summary>
    public class Itdma {
        public Itdma() {
        }

        public Itdma(ISixbit sixState) {
            Parse(sixState);
        }

        /// <summary>
        ///     2 bits   : ITDMA Sync State
        /// </summary>
        public int SyncState { get; internal set; }

        /// <summary>
        ///     13 bits  : ITDMA Slot Increment
        /// </summary>
        public int SlotInc { get; internal set; }

        /// <summary>
        ///     3 bits   : ITDMA Number of Slots
        /// </summary>
        public int NumSlots { get; internal set; }

        /// <summary>
        ///     1 bit    : ITDMA Keep Flag
        /// </summary>
        public int KeepFlag { get; internal set; }

        public void Parse(ISixbit sixState) {
            if (sixState.BitLength< 19) throw new AisMessageException("ITDMA wrong length");

            SyncState = (char) sixState.Get(2);
            SlotInc = (int) sixState.Get(13);
            NumSlots = (char) sixState.Get(3);
            KeepFlag = (char) sixState.Get(1);
        }
    }
}