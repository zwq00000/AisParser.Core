namespace AisParser
{
    /// <summary>
    ///     AIS Message base class
    ///     All the messages are derived from this class which provides the msgid,
    ///     repeat value and userid
    /// </summary>
    public abstract class Messages {
        protected Messages (int msgId) {
            MsgId = msgId;
        }

        protected Messages (int msgId, int repeat, long userId) : this (msgId) {
            Repeat = repeat;
            UserId = userId;
        }

        /// <summary>
        ///     6 bits  : Message ID (1)
        /// </summary>
        public int MsgId { get; }

        /// <summary>
        ///     2 bits  : Repeated
        /// </summary>
        public int Repeat { get; internal set; }

        /// <summary>
        ///     30 bits : UserID / MMSI
        /// </summary>
        public long UserId { get; internal set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        public virtual void Parse (ISixbit sixState) {
            Repeat = (int) sixState.Get (2);
            UserId = sixState.Get (30);
        }

        public override string ToString () {
            return $"Message{MsgId} {{UserID:{UserId} , Repeat:{Repeat} }}";
        }
    }
}