namespace AisParser {
    /// <summary>
    ///     AIS Message 19 class
    ///     Extended Class B Equipment Position Report
    /// </summary>
    public sealed class Message19 : Messages {
        public Message19():base(19) {
		}

		public Message19(ISixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     8 bits   : Regional Bits
        /// </summary>
        public int Regional1 { get; internal set; }

        /// <summary>
        ///     10 bits  : Speed Over Ground
        /// </summary>
        public int Sog { get; internal set; }

        /// <summary>
        ///     1 bit    : Position Accuracy
        /// </summary>
        public int PosAcc { get; internal set; }

        /// <summary>
        ///     : Lat/Long 1/100000 minute
        /// </summary>
        public Position Pos { get; internal set; }

        /// <summary>
        ///     12 bits  : Course Over Ground
        /// </summary>
        public int Cog { get; internal set; }

        /// <summary>
        ///     9 bits   : True Heading
        /// </summary>
        public int TrueHeading { get; internal set; }

        /// <summary>
        ///     6 bits   : UTC Seconds
        /// </summary>
        public int UtcSec { get; internal set; }

        /// <summary>
        ///     4 bits   : Regional Bits
        /// </summary>
        public int Regional2 { get; internal set; }

        /// <summary>
        ///     120 bits : Ship Name in ASCII
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        ///     8 bits   : Type of Ship and Cargo
        /// </summary>
        public int ShipType { get; internal set; }

        /// <summary>
        ///     9 bits   : GPS Ant. Distance from Bow
        /// </summary>
        public int DimBow { get; internal set; }

        /// <summary>
        ///     9 bits   : GPS Ant. Distance from Stern
        /// </summary>
        public int DimStern { get; internal set; }

        /// <summary>
        ///     6 bits   : GPS Ant. Distance from Port
        /// </summary>
        public int DimPort { get; internal set; }

        /// <summary>
        ///     6 bits   : GPS Ant. Distance from Starboard
        /// </summary>
        public int DimStarboard { get; internal set; }

        /// <summary>
        ///     4 bits   : Type of Position Fixing Device
        /// </summary>
        public int PosType { get; internal set; }

        /// <summary>
        ///     1 bit    : RAIM Flag
        /// </summary>
        public int Raim { get; internal set; }

        /// <summary>
        ///     1 bit    : DTE Flag
        /// </summary>
        public int Dte { get; internal set; }

        /// <summary>
        ///     5 bits   : Spare
        /// </summary>
        public int Spare { get; internal set; }


        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            if (sixState.BitLength!= 312) throw new AisMessageException("Message 19 wrong length");

            base.Parse(sixState);

            Regional1 = (int) sixState.Get(8);
            Sog = (int) sixState.Get(10);
            PosAcc = (int) sixState.Get(1);

            Pos = Position.FromAis(longitude:sixState.Get(28),latitude:sixState.Get(27));

            Cog = (int) sixState.Get(12);
            TrueHeading = (int) sixState.Get(9);
            UtcSec = (int) sixState.Get(6);
            Regional2 = (int) sixState.Get(4);
            Name = sixState.GetString(20);
            ShipType = (int) sixState.Get(8);
            DimBow = (int) sixState.Get(9);
            DimStern = (int) sixState.Get(9);
            DimPort = (int) sixState.Get(6);
            DimStarboard = (int) sixState.Get(6);
            PosType = (int) sixState.Get(4);
            Raim = (int) sixState.Get(1);
            Dte = (int) sixState.Get(1);
            Spare = (int) sixState.Get(5);
        }
    }
}