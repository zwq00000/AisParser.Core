namespace AisParser {
    /// <summary>
    ///     AIS Message 12 class
    ///     Addressed Safety Related Message
    /// </summary>
    public sealed class Message12 : Messages {
        public Message12():base(12) {
		}

		public Message12(ISixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits   : Sequence
        /// </summary>
        public int Sequence { get; internal set; }

        /// <summary>
        ///     30 bits  : Destination MMSI
        /// </summary>
        public long Destination { get; internal set; }

        /// <summary>
        ///     1 bit    : Retransmit
        /// </summary>
        public int Retransmit { get; internal set; }

        /// <summary>
        ///     1 bit    : Spare
        /// </summary>
        public int Spare { get; internal set; }

        /// <summary>
        ///     936 bits : Message in ASCII
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            var length = sixState.BitLength;
            if (length < 72 || length > 1008) throw new AisMessageException("Message 12 wrong length");

            base.Parse(sixState);

            Sequence = (int) sixState.Get(2);
            Destination = sixState.Get(30);
            Retransmit = (int) sixState.Get(1);
            Spare = (int) sixState.Get(1);

            Message = sixState.GetString((length - 72) / 6);
        }
    }
}