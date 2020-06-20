using System.Buffers;
using System.Runtime.InteropServices;
/**
*! \file
\brief 6-bit packed ASCII functions
\author Copyright 2006-2008 by Brian C. Lane <bcl@brianlane.com>, All Rights Reserved
\version 1.8
*/
namespace AisParser.Tests
{

    /// <summary>
    ///     该模块的功能用于从6位压缩包中提取数据AIVDM / AIVDO AIS消息使用的ASCII字符串。
    /// sixbit机器的状态保持在sixbit结构中，因此可以通过维护单独的实例来处理多个流的六位 init_6bit（）应该使用指向六位的指针来调用实例，
    ///     它将设置用于解析的结构。6位数据然后应将其复制到sixbit.bits缓冲区中。最多255
    /// 允许使用字符（这由#SIXBIT_LEN在sixbit.h中更改）  通过调用get_6bit（）从字符串中获取最多32位数据数据包的大小可以用strlen（state-> bits）* 6计算，
    /// 但是请注意，由于字符串末尾的填充，数据可能会比消息ID的预期长度长1-6位。
    // 由调用函数决定跟踪多少位被获取。当到达6位字符串的末尾时，get_6bit（）将返回0。
    /// </summary>
    /// <remarks>
    /// This module's functions are used to extract data from the 6-bit packed
    /// ASCII string used by AIVDM/AIVDO AIS messages.
    ///  
    /// The state of the sixbit machine is held in the sixbit structure so that
    /// multiple streams can be processed by maintaining separate instances
    /// of sixbit. init_6bit() should be called with a pointer to the sixbit
    /// instance, it will setup the structure for parsing. The 6-bit data 
    /// should then be copied into the sixbit.bits buffer. A maximum of 255
    /// characters are allowed (this is changed by #SIXBIT_LEN in sixbit.h)
    /// 	
    /// Up to 32 bits of data are fetched from the string by calling get_6bit()
    /// The size of the packet can be calculated with strlen(state->bits) * 6 but
    /// note that due to padding at the end of the string the data may be 
    /// 1-6 bits longer than the the expected length for the message id.
    /// It is up to the calling function to keep track of how many bits have
    /// been fetched. When it reaches the end of the 6-bit string get_6bit()
    /// will return 0's.
    /// </remarks>
    internal partial class SixBits {

        /* ----------------------------------------------------------------------- */
        /** Convert a binary value to a 6-bit ASCII value
            This function checks the binary value to make sure it can be coverted.
            If not, it returns a -1.
            Otherwise it returns the 6-bit ASCII value.
            \param value to convert
            returns:
              - -1 if it fails
              - 6-bit ASCII
            This is used to convert a 6 bit binary value to the packed 6-bit value 
            It is not used to convert data from fields such as the name and 
            destination -- Use ais2ascii() instead.
        */
        /* ----------------------------------------------------------------------- */

        /// <summary>
        /// Convert a binary value to a 6-bit ASCII value
        /// </summary>
        /// <remarks>
        /// This function checks the binary value to make sure it can be coverted.
        /// If not, it returns a -1.
        /// Otherwise it returns the 6-bit ASCII value.
        /// \param value to convert
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>
        /// This is used to convert a 6 bit binary value to the packed 6-bit value 
        /// It is not used to convert data from fields such as the name and destination -- Use ais2ascii() instead.
        ///  - -1 if it fails
        ///  - 6-bit ASCII
        /// </returns>
        static int BinTo6bit (byte value) {
            if (value > 0x3F)
                return -1;
            if (value < 0x28)
                return value + 0x30;
            else
                return value + 0x38;
        }

        /* ----------------------------------------------------------------------- */
        /** Initialize a 6-bit datastream structure
            \param state state of 6-bit parser
            This function initializes the state of the sixbit parser variables 
            and 0 terminates the 6-bit string.
            
            Example:
            \code
            sixbit state;
            
            init_6bit( &state );
            \endcode
        */
        /* ----------------------------------------------------------------------- */
        // static int init_6bit (ref sixbit state) {
        //     state.remainder = 0;
        //     state.remainder_bits = 0;
        //     //state.p = state.bits;
        //     //state.p = 0;

        //     return 0;
        // }

    }
}