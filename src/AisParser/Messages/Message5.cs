namespace AisParser {
    /// <summary>
    ///     AIS Message 5 class
    ///     Static and Voyage Related Data
    /// </summary>
    public sealed class Message5 : Messages {
        public Message5():base(5) {
		}

		public Message5(ISixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits          : AIS Version
        /// </summary>
        public int Version { get; internal set; }

        /// <summary>
        ///     30 bits         : IMO Number
        /// </summary>
        public long Imo { get; internal set; }

        /// <summary>
        ///     7x6 (42) bits   : Callsign
        /// </summary>
        public string Callsign { get; internal set; }

        /// <summary>
        ///     20x6 (120) bits : Ship Name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        ///     8 bits          : Type of Ship and Cargo
        /// </summary>
        public int ShipType { get; internal set; }

        /// <summary>
        ///     9 bits          : GPS Ant. Distance from Bow
        /// </summary>
        public int DimBow { get; internal set; }

        /// <summary>
        ///     9 bits          : GPS Ant. Distance from stern
        /// </summary>
        public int DimStern { get; internal set; }

        /// <summary>
        ///     6 bits          : GPS Ant. Distance from port
        /// </summary>
        public int DimPort { get; internal set; }

        /// <summary>
        ///     6 bits          : GPS Ant. Distance from starboard
        /// </summary>
        public int DimStarboard { get; internal set; }

        /// <summary>
        ///     4 bits          : Type of position fixing device
        /// </summary>
        public int PosType { get; internal set; }

        /// <summary>
        ///     20 bits         : Estimated Time of Arrival MMDDHHMM
        /// </summary>
        public long Eta { get; internal set; }

        /// <summary>
        ///     8 bits          : Maximum present static draught
        /// </summary>
        public int Draught { get; internal set; }

        /// <summary>
        ///     6x20 (120) bits : Ship Destination
        /// </summary>
        public string Dest { get; internal set; }

        /// <summary>
        ///     1 bit           : DTE flag
        /// </summary>
        public int Dte { get; internal set; }

        /// <summary>
        ///     1 bit           : spare
        /// </summary>
        public int Spare { get; internal set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            if (sixState.BitLength!= 424) throw new AisMessageException("Message 5 wrong length");

            base.Parse(sixState);

            Version = (int) sixState.Get(2);
            Imo = sixState.Get(30);
            Callsign = sixState.GetString(7);
            Name = sixState.GetString(20);
            ShipType = (int) sixState.Get(8);
            DimBow = (int) sixState.Get(9);
            DimStern = (int) sixState.Get(9);
            DimPort = (int) sixState.Get(6);
            DimStarboard = (int) sixState.Get(6);
            PosType = (int) sixState.Get(4);
            Eta = sixState.Get(20);
            Draught = (int) sixState.Get(8);
            Dest = sixState.GetString(20);
        }
    }
}