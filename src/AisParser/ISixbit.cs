namespace AisParser {
    public interface ISixbit {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        int Length { get; }

        /// <summary>
        /// Takes into account the number of padding bits.
        /// </summary>
        /// <value></value>
        int BitLength { get; }

        /// <summary>
        /// Return 0-32 bits from a 6-bit ASCII stream
        /// </summary>
        /// <param name="numbits"></param>
        /// <returns></returns>
        long Get (int numbits);
        /// <summary>
        /// Get an ASCII string from the 6-bit data stream
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        string GetString (int length);

        /// <summary>
        /// Set the bit padding value
        /// </summary>
        /// <param name="num"></param>
        void PadBits (int num);
    }

    public interface ISixbit<T> : ISixbit {

        /// <summary>
        ///     Add more bits to the buffer
        /// </summary>
        void Add (T bits);

    }
}