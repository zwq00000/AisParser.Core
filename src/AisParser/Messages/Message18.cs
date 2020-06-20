namespace AisParser {
    /// <summary>
    ///     AIS Message 18 class
    ///     Standard Class B equipment position report
    /// </summary>
    public sealed class Message18 : Messages {
        public Message18 () : base (18) { }

        public Message18 (ISixbit sixbit) : this () {
            this.Parse (sixbit);
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
        ///     2 bits   : Regional Bits
        /// </summary>
        public int Regional2 { get; internal set; }

        /// <summary>
        ///     1 bit    : Class B CS Flag
        /// </summary>
        public int UnitFlag { get; internal set; }

        /// <summary>
        ///     1 bit    : Integrated msg14 Display Flag
        /// </summary>
        public int DisplayFlag { get; internal set; }

        /// <summary>
        ///     1 bit    : DSC Capability flag
        /// </summary>
        public int DscFlag { get; internal set; }

        /// <summary>
        ///     1 bit    : Marine Band Operation Flag
        /// </summary>
        public int BandFlag { get; internal set; }

        /// <summary>
        ///     1 bit    : Msg22 Frequency Management Flag
        /// </summary>
        public int Msg22Flag { get; internal set; }

        /// <summary>
        ///     1 bit    : Autonomous Mode Flag
        /// </summary>
        public int ModeFlag { get; internal set; }

        /// <summary>
        ///     1 bit    : RAIM Flag
        /// </summary>
        public int Raim { get; internal set; }

        /// <summary>
        ///     1 bit    : Comm State Flag
        /// </summary>
        public int CommState { get; internal set; }

        public Sotdma SotdmaState { get; internal set; }
        public Itdma ItdmaState { get; internal set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse (ISixbit sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException ("Message 18 wrong length");

            base.Parse (sixState);

            Regional1 = (int) sixState.Get (8);
            Sog = (int) sixState.Get (10);
            PosAcc = (int) sixState.Get (1);

            Pos = Position.FromAis (
                longitude: sixState.Get (28),
                latitude: sixState.Get (27)
            );

            Cog = (int) sixState.Get (12);
            TrueHeading = (int) sixState.Get (9);
            UtcSec = (int) sixState.Get (6);
            Regional2 = (int) sixState.Get (2);
            UnitFlag = (int) sixState.Get (1);
            DisplayFlag = (int) sixState.Get (1);
            DscFlag = (int) sixState.Get (1);
            BandFlag = (int) sixState.Get (1);
            Msg22Flag = (int) sixState.Get (1);
            ModeFlag = (int) sixState.Get (1);
            Raim = (int) sixState.Get (1);
            CommState = (int) sixState.Get (1);

            if (CommState == 0)
                SotdmaState = new Sotdma (sixState);
            else
                ItdmaState = new Itdma (sixState);
        }
    }
}