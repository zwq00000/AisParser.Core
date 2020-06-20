using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;

namespace AisParser {

    /// <summary>
    /// 6-bit packed ASCII functions for span
    /// </summary>
    public ref struct SixbitSpan {
        const int SIXBIT_LEN = 255;
        static readonly byte[] pow2_mask = { 0x00, 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F };

        /// <summary>
        /// raw 6-bit ASCII data
        /// </summary>
        private ReadOnlySpan<byte> _bits;

        private ReadOnlySpan<byte> _bits2;

        /// <summary>
        /// six bit byte lenght, _bits.length * 6 + _bits2 * 6 
        /// </summary>
        private int _bitLength;

        private int _index;

        /// <summary>
        /// pointer to current character in bits
        /// </summary>
        public byte Point => _index < _bits.Length? _bits[_index] : _bits2[_index - _bits.Length];

        /// <summary>
        /// Remainder bits
        /// </summary>
        private int remainder;

        /// <summary>
        /// Number of remainder bits
        /// </summary>
        private int remainder_bits;
        private int _padBits;

        public SixbitSpan (ReadOnlySpan<byte> buf) {
            this._bits = buf;
            this._bits2 = null;
            remainder = 0;
            remainder_bits = 0;
            _padBits = 0;
            this._index = 0;
            _bitLength = buf.Length * 6;
        }

        /// <summary>
        ///     Set the bit padding value
        /// </summary>
        public void PadBits (int num) {
            _padBits = num;
        }

        /// <summary>
        ///     Add more bits to the buffer
        /// </summary>
        public void Add (ReadOnlySpan<byte> bits) {
            if (_bits2 != null) {
                throw new IndexOutOfRangeException ("_bits2 ex");
            }
            if (this._bits.Length == 0) {
                this._bits = bits;
                _bitLength = _bits.Length;
            } else {
                this._bits2 = bits;
                _bitLength = _bits.Length + _bits2.Length;
            }
        }

        /// <summary>
        /// Calculate the number of bits remaining in the six_state
        /// </summary>
        /// <returns>Number of bits remaining</returns>
        public int Length => _bitLength;

        /// <summary>
        ///     Takes into account the number of padding bits.
        /// </summary>
        /// <returns>Return the number of bits</returns>
        public int BitLength => Length * 6 - _padBits;
       
        /// <summary>
        /// Return 0-32 bits from a 6-bit ASCII stream 
        /// </summary>
        /// <remark>
        /// This function returns the requested number of bits to the calling function. 
        /// It pulls the bits from the raw 6-bit ASCII as they are needed.
        /// The full string can be addressed by pointing to state->bits and the length can be calculated by strlen(state->bits) * 6, 
        /// but note that the string also includes any final padding, so when checking lengths take into account that it will be a multiples of 6 bits.
        /// </remark>
        /// <example>
        /// <code>
        ///      sixbit  state;
        ///      unsigned char i;
        ///      init_6bit( state );
        ///      strcpy( state.bits, "5678901234" );
        ///      i = get_6bit( state, 6 );
        ///      i == 5
        /// </code>
        /// </example>
        /// <param name="numbits">number of bits to return</param>
        /// <returns>long value</returns>
        public long Get (int numbits) {
            Debug.WriteLine("sixbit get {0}",numbits);
            long result;
            int fetch_bits;

            result = 0;
            fetch_bits = numbits;

            while (fetch_bits > 0) {
                /*  Is there anything left over from the last call? */
                if (remainder_bits > 0) {
                    if (remainder_bits <= fetch_bits) {
                        /* reminder is less than or equal to what is needed */
                        result = (result << 6) + remainder;
                        fetch_bits -= remainder_bits;
                        remainder = 0;
                        remainder_bits = 0;
                    } else {
                        /* remainder is larger than what is needed
                           Take the bits from the top of remainder
                        */
                        result = result << fetch_bits;
                        result += remainder >> (remainder_bits - fetch_bits);

                        /* Fixup remainder */
                        remainder_bits -= fetch_bits;
                        remainder &= pow2_mask[(int) remainder_bits];

                        return result;
                    }
                }

                /* Get the next block of 6 bits from the ASCII string */
                if (_index < Length) {
                    remainder = BinFrom6bit (Point);
                    remainder_bits = 6;
                    _index++;
                } else {
                    /* Nothing more to fetch, return what we have */
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        ///     Get an ASCII string from the 6-bit data stream
        /// </summary>
        /// <param name="length"> Number of characters to retrieve </param>
        /// <returns> String of the characters</returns>
        public string GetString (int length) {
            var tmpStr = new char[length];
            int len = 0;
            // Get the 6-bit string, convert to ASCII
            for (var i = 0; i < length; i++) {
                if (TryToChar (Get (6), out var c)) {
                    if (c == '@') {
                        //skip '@' char
                        continue;
                    } else {
                        tmpStr[i] = c;
                        len++;
                    }
                } else {
                    //for (var j = i; j < length; j++) tmpStr[j] = '@';
                    break;
                }
            }
            return new string (tmpStr, 0, len);
        }

        #region static methods
        /// <summary>
        /// Convert an ascii value to a 6-bit binary value
        /// This function checks the ASCII value to make sure it can be coverted.
        /// If not, it returns a -1.
        /// Otherwise it returns the 6-bit binary value.
        /// <c>ascii</c> character to convert
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns> 
        ///  -1 if it fails
        ///   6-bit value (0x00-0x3F)</returns>
        /// <remarks>
        /// This is used to convert the packed 6-bit value to a binary value. 
        /// It is not used to convert data from fields such as the name and 
        /// destination -- Use ais2ascii() instead.
        /// </remarks>
        private static int BinFrom6bit (byte ascii) {
            if ((ascii < 0x30) || (ascii > 0x77) || ((ascii > 0x57) && (ascii < 0x60)))
                return -1;
            if (ascii < 0x60)
                return (ascii - 0x30) & 0x3F;
            else
                return (ascii - 0x38) & 0x3F;
        }

        /// <summary>
        /// Convert a AIS 6-bit character to ASCII
        /// </summary>
        /// <param name="value">
        ///     6-bit value to be converted
        ///     return
        ///     - corresponding ASCII value (0x20-0x5F)     *
        ///     This function is used to convert binary data to ASCII. This is
        ///     different from the 6-bit ASCII to binary conversion for VDM
        ///     messages; it is used for strings within the datastream itself.
        ///     eg. Ship Name, Callsign and Destination.
        /// </param>
        /// <param name="c">get the char</param>
        /// <exception cref="ArgumentException">value &gt; 0x3F</exception>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        private static bool TryToChar (long value, out char c) {
            if (value > 0x3F) {
                c = '\0';
                return false;
            }
            if (value < 0x20) {
                c = (char) (value + 0x40);
            } else {
                c = (char) value;
            }
            return true;
        }

        #endregion
    }
}