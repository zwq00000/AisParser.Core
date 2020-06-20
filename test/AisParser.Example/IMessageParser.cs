using System;
using System.Buffers;

namespace AisParser.Example
{
    public interface IMessageParser{
         /// <summary>
        /// 尝试从<see ref="ReadOnlySequence{byte}"></see>中读取数据包
        /// </summary>
        /// <param name="buffer">消息流</param>
        /// <param name="message">解析的消息</param>
        /// <param name="consumed">消费字节数量</param>
        /// <returns></returns>
        bool TryParse(ref ReadOnlySequence<byte> buffer, out Messages message, out SequencePosition consumed);

    }
}