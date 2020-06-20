namespace AisParser {
    /// <summary>
    ///     AIS Message 1 class
    ///     Position Report
    /// </summary>
    public sealed class Message1 : Message123 {
        public Message1() : base(1) {
        }

        public Message1(ISixbit sixbit) : this() {
            this.Parse(sixbit);
        }


        /// <summary>
        ///     3 bits  : SOTDMA Slot Timeout
        /// </summary>
        public int SlotTimeout { get; internal set; }

        /// <summary>
        ///     14 bits : SOTDMA sub-message
        /// </summary>
        public int SubMessage { get; internal set; }

        /// <summary>
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException("Message 1 wrong length");

            base.Parse(sixState);

            /* Parse the Message 1 */
            SlotTimeout = (int) sixState.Get(3);
            SubMessage = (int) sixState.Get(14);
        }
    }
}