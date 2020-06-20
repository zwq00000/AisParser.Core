namespace AisParser {
    /// <summary>
    ///     AIS Message 11 class
    ///     UTC/Date response
    /// </summary>
    public sealed class Message11 : Messages {
        public Message11():base(11) {
		}

		public Message11(ISixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     14 bits : UTC Year
        /// </summary>
        public int UtcYear { get; internal set; }

        /// <summary>
        ///     4 bits  : UTC Month
        /// </summary>
        public int UtcMonth { get; internal set; }

        /// <summary>
        ///     5 bits  : UTC Day
        /// </summary>
        public int UtcDay { get; internal set; }

        /// <summary>
        ///     5 bits  : UTC Hour
        /// </summary>
        public int UtcHour { get; internal set; }

        /// <summary>
        ///     6 bits  : UTC Minute
        /// </summary>
        public int UtcMinute { get; internal set; }

        /// <summary>
        ///     6 bits  : UTC Second
        /// </summary>
        public int UtcSecond { get; internal set; }

        /// <summary>
        ///     1 bit   : Position Accuracy
        /// </summary>
        public int PosAcc { get; internal set; }

        /// <summary>
        ///     : Lat/Long 1/10000 minute
        /// </summary>
        public Position Pos { get; internal set; }

        /// <summary>
        ///     4 bits  : Type of position fixing device
        /// </summary>
        public int PosType { get; internal set; }

        /// <summary>
        ///     10 bits : Spare
        /// </summary>
        public int Spare { get; internal set; }

        /// <summary>
        ///     1 bit   : RAIM flag
        /// </summary>
        public int Raim { get; internal set; }

        public Sotdma SotdmaState { get; internal set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            if (sixState.BitLength!= 168) throw new AisMessageException("Message 11 wrong length");

            base.Parse(sixState);

            /* Parse the Message 11 */
            UtcYear = (int) sixState.Get(14);
            UtcMonth = (int) sixState.Get(4);
            UtcDay = (int) sixState.Get(5);
            UtcHour = (int) sixState.Get(5);
            UtcMinute = (int) sixState.Get(6);
            UtcSecond = (int) sixState.Get(6);
            PosAcc = (int) sixState.Get(1);

            Pos = Position.FromAis(longitude:sixState.Get(28),latitude:sixState.Get(27));

            PosType = (int) sixState.Get(4);
            Spare = (int) sixState.Get(10);
            Raim = (int) sixState.Get(1);
            SotdmaState = new Sotdma();
            SotdmaState.Parse(sixState);
        }
    }
}