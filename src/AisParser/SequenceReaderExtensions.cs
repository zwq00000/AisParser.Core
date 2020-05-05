using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace AisParser {
    internal static class SequenceReaderExtensions {

        /// <summary>
        /// 异或计算
        /// </summary>
        /// <param name="span"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static byte XOR (this ReadOnlySpan<byte> span, byte initValue = 0) {
            for (int i = 0; i < span.Length; i++) {
                initValue ^= span[i];
            }
            return initValue;
        }

        /// <summary>
        /// 异或计算
        /// </summary>
        /// <param name="span"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static byte XOR (this Span<byte> span, byte initValue = 0) {
            for (int i = 0; i < span.Length; i++) {
                initValue ^= span[i];
            }
            return initValue;
        }

        /// <summary>
        /// 搜索连续的分隔符，并向前越过找到的连续分隔符
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="delimiters">连续的分隔符</param>
        /// <param name="advancePastDelimiter">若要越过 true（如果已发现），则为 delimiter；否则为 false</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryAdvanceToSequence<T> (this ref SequenceReader<T> reader, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true) where T : unmanaged, IEquatable<T> {
            if (delimiters == null || delimiters.Length == 0) {
                throw new ArgumentNullException (nameof (delimiters));
            }
            if (delimiters.Length == 1) {
                return reader.TryAdvanceTo (delimiters[0], advancePastDelimiter);
            }
            var originalPostion = reader.Consumed;
            var first = delimiters[0];
            while (!reader.End) {
                RESEARCH : if (reader.TryAdvanceTo (first, true)) {
                    for (var i = 1; i < delimiters.Length; i++) {
                        if (!reader.IsNext (delimiters[i], true)) {
                            //不满足连续分隔符要求,从第一个分隔符开始继续搜索
                            goto RESEARCH;
                        }
                    }
                    if (!advancePastDelimiter) {
                        //回退到原始位置
                        reader.Rewind (reader.Consumed - originalPostion);
                    }
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 搜索连续的分隔符，并向读取前越过找到的连续分隔符部分
        /// 一个分隔符时 等同于 <see ref="SequenceReader{T}.TryReadTo"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="sequence">read part</param>
        /// <param name="delimiters">连续的分隔符</param>
        /// <param name="advancePastDelimiter">若要越过 true（如果已发现），则为 delimiter；否则为 false</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryReadToSequence<T> (this ref SequenceReader<T> reader, out ReadOnlySequence<T> sequence, ReadOnlySpan<T> delimiters, bool advancePastDelimiter = true) where T : unmanaged, IEquatable<T> {
            sequence = ReadOnlySequence<T>.Empty;
            if (delimiters == null || delimiters.Length == 0) {
                throw new ArgumentNullException (nameof (delimiters));
            }
            if (delimiters.Length == 1) {
                return reader.TryReadTo (out sequence, delimiters[0], advancePastDelimiter);
            }
            var originalPostion = reader.Consumed;
            var first = delimiters[0];
            while (!reader.End) {
                RESEARCH : if (reader.TryAdvanceTo (first, true)) {
                    for (var i = 1; i < delimiters.Length; i++) {
                        if (!reader.IsNext (delimiters[i], true)) {
                            //不满足连续分隔符要求,从第一个分隔符开始继续搜索
                            goto RESEARCH;
                        }
                    }
                    var length = reader.Consumed - originalPostion;
                    sequence = reader.Sequence.Slice (originalPostion, length);
                    if (!advancePastDelimiter) {
                        //回退到原始位置
                        reader.Rewind (length);
                    }
                    return true;
                }
                else {
                    return false;
                }
            }

            return false;
        }
    }
}