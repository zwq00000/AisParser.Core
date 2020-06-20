using System;

namespace AisParser {
    /// <summary>
    ///     AIS Message 1/2/3 class
    ///     Position Report
    /// </summary>
    public abstract class Message123 : Messages {
        protected Message123 (int msgId) : base (msgId) {
            if (msgId != 1 && msgId != 2 && msgId != 3) {
                throw new ArgumentException ("message Id must be [1,2,3]", nameof (msgId));
            }
        }

        protected Message123 (int navStatus, int rot, int sog, int posAcc, Position pos, int cog, int trueHeading, int utcSec, int regional, int spare, int raim, int syncState) : this (navStatus) {
            Rot = rot;
            Sog = sog;
            PosAcc = posAcc;
            Pos = pos;
            Cog = cog;
            TrueHeading = trueHeading;
            UtcSec = utcSec;
            Regional = regional;
            Spare = spare;
            Raim = raim;
            SyncState = syncState;
        }

        /// <summary>
        ///     4 bits  : Navigational Status
        /// </summary>
        public int NavStatus { get; internal set; }

        /// <summary>
        ///     8 bits  : Rate of Turn
        /// </summary>
        public int Rot { get; internal set; }

        /// <summary>
        ///     10 bits : Speed Over Ground
        /// </summary>
        public int Sog { get; internal set; }

        /// <summary>
        ///     1 bit   : Position Accuracy
        /// </summary>
        public int PosAcc { get; internal set; }

        /// <summary>
        ///     : Lat/Long 1/10000 minute
        /// </summary>
        public Position Pos { get; internal set; }

        /// <summary>
        ///     12 bits : Course over Ground
        /// </summary>
        public int Cog { get; internal set; }

        /// <summary>
        ///     9 bits  : True heading
        /// </summary>
        public int TrueHeading { get; internal set; }

        /// <summary>
        ///     6 bits  : UTC Seconds
        /// </summary>
        public int UtcSec { get; internal set; }

        /// <summary>
        ///     4 bits  : Reserved for regional
        /// </summary>
        /// <remarks>
        /// manoeuvre indicator 
        ///     0 = not available = default 
        ///     1 = not engaged in special manoeuvre 
        ///     2 = engaged in special manoeuvre (i.e. regional passing arrangement on Inland Waterway) 
        /// </remarks>
        public int Regional { get; internal set; }

        /// <summary>
        ///     1 bit   : Spare
        /// </summary>
        public int Spare { get; internal set; }

        /// <summary>
        ///     1 bit   : RAIM flag
        /// </summary>
        public int Raim { get; internal set; }

        /// <summary>
        ///     2 bits  : SOTDMA sync state
        /// </summary>
        public int SyncState { get; internal set; }

        /// <summary>
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse (ISixbit sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException ($"Message {MsgId} wrong length");

            base.Parse (sixState);

            /* Parse the Message 1 */
            NavStatus = (int) sixState.Get (4);
            Rot = (int) sixState.Get (8);
            Sog = (int) sixState.Get (10);
            PosAcc = (int) sixState.Get (1);

            Pos = Position.FromAis (longitude: sixState.Get (28), latitude: sixState.Get (27));

            Cog = (int) sixState.Get (12);
            TrueHeading = (int) sixState.Get (9);
            UtcSec = (int) sixState.Get (6);
            Regional = (int) sixState.Get (2);
            Spare = (int) sixState.Get (3);
            Raim = (int) sixState.Get (1);
            SyncState = (int) sixState.Get (2);
        }

        public override string ToString () {
            return $"Message{MsgId} {{UserID:{UserId} , NavStatus:{NavStatus}, Rot:{Rot}, Sog:{Sog}, PosAcc:{PosAcc},Pos:{Pos},Cog:{Cog},Heading:{TrueHeading},UtcSec:{UtcSec},Regional:{Regional},Spare:{Spare},Raim:{Raim} }}";
        }
    }
}