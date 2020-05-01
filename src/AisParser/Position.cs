namespace AisParser {
    /// <summary>
    ///     AIS Position class
    ///     Convert raw unsigned AIS position to signed 1/10000 degree position
    ///     and provide helper methods for other formats
    /// </summary>
    public class Position {
        private long _latitude;
        private long _longitude;

        //public Position () { }

        // public Position (long longitude, long latitude) {
        //     this.Longitude = longitude;
        //     this.Latitude = latitude;
        // }

        public Position (double longitude, double latitude) {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        private static long FixLongitude (long value) {
            // Convert longitude to signed number 
            if (value >= 0x8000000) {
                return -(0x10000000 - value);
            } else {
                return value;
            }
        }

        private static long FixLatitude (long value) {
            // Convert latitude to signed number 
            if (value >= 0x4000000) {
                return -(0x8000000 - value);
            } else {
                return value;
            }
        }

        public double Longitude { get; }

        public double Latitude { get; }

        public static Position FromAis (long longitude, long latitude) {
            var lng = FixLongitude (longitude) / 600000d;
            var lat = FixLatitude (latitude) / 600000d;
            return new Position (lng, lat);
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString () {
            return $"{{lng:{Longitude},lat:{Latitude}";
        }

        /// <summary>
        /// Get Coordinates [longitude,latitude]
        /// </summary>
        /// <returns></returns>
        public double[] ToCoordinates () {
            return new double[] { Longitude, Latitude };
        }

        #endregion
    }
}