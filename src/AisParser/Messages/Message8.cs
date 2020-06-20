namespace AisParser {
    /// <summary>
    ///     AIS Message 8 class
    ///     Binary Broadcast Message
    /// </summary>
    public sealed class Message8 : Messages {
        public Message8():base(8) {
		}

		public Message8(ISixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare { get; internal set; }

        /// <summary>
        ///     16 bits  : Application ID
        /// </summary>
        public int AppId { get; internal set; }

        /// <summary>
        ///     952 bits : Data payload
        /// </summary>
        public ISixbit Data { get; internal set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            var length = sixState.BitLength;
            if (length < 56 || length > 1008) throw new AisMessageException("Message 8 wrong length");

            base.Parse(sixState);

            Spare = (int) sixState.Get(2);
            AppId = (int) sixState.Get(16);

            /* Store the remaining payload of the packet for further processing */
            Data = sixState;
        }
    }
}