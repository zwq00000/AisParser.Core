namespace AisParser
{
    public enum VdmStatus {
        /// <summary>
        /// Complete packet
        /// </summary>
        Complete = 0,

        /// <summary>
        /// Incomplete packet
        /// </summary>
        Incomplete = 1,

        /// <summary>
        /// NMEA 0183 checksum failed
        /// </summary>
        ChecksumFailed = 2,

        /// <summary>
        /// Not an AIS message
        /// </summary>
        NotAisMessage = 3,

        /// <summary>
        /// Error with nmea_next_field
        /// </summary>
        NmeaNextError = 4,

        /// <summary>
        ///  Out of sequence packet
        /// </summary>
        OutofSequence = 5,

        ///<summary>
        /// Message parse format error
        ///</summary>
        FormatError = 6
    }
}