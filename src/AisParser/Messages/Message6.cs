namespace AisParser {
    /// <summary>
    ///    AIS Message 6 class
    ///    Binary Addressed Message
    /// </summary>
    public sealed class Message6 : Messages {
        public Message6():base(6) {
		}

		public Message6(ISixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///  2 bits : Sequence number
        /// </summary>
        /// <remarks>
        /// 序列编号 0-3:参考附件2的第5.3.1节 
        /// </remarks>
        public int Sequence { get; internal set; }

        /// <summary>
        /// 30 bits : Destination MMSI
        /// </summary>
        /// <remarks>
        /// 目的地台站的MMSI编号 
        /// </remarks>
        public long Destination { get; internal set; }

        /// <summary>
        ///    1 bit : Retransmit
        /// </summary>
        /// <remarks>
        /// 重发标志应根据重发情况设置:
        /// 0 = 无重发 = 默认值
        /// 1 = 已重发 
        /// </remarks>
        public bool Retransmit { get; internal set; }

        /// <summary>
        ///    1 bit : Spare
        /// </summary>
        public int Spare { get; internal set; }

        /// <summary>
        ///    16 bits : Application ID
        /// </summary>
        public int AppId { get; internal set; }

        /// <summary>
        ///    960 bits : Data payload
        /// </summary>
        public ISixbit Data { get; internal set; }

        /// <summary>
        ///    Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(ISixbit sixState) {
            if (sixState.BitLength< 88 || sixState.BitLength> 1008)
                throw new AisMessageException("Message 6 wrong length");

            base.Parse(sixState);

            Sequence = (int) sixState.Get(2);
            Destination = sixState.Get(30);
            Retransmit = sixState.Get(1)==1;
            Spare = (int) sixState.Get(1);
            AppId = (int) sixState.Get(16);

            /* Store the remaining payload of the packet for further processing */
            Data = sixState;
        }
    }
}