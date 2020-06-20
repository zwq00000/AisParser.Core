using System;
using System.Buffers;
using System.Threading.Tasks;

namespace AisParser.Example {
    public class MessageParser : IMessageParser {
        public bool TryParse (ref ReadOnlySequence<byte> buffer, out Messages message, out SequencePosition consumed) {
            var reader = new SequenceReader<byte> (buffer);
            
            if (VdmParser.TryParse (ref reader, out var vdm, out var sixbit)) {
                message = MessageBuilder.Build (vdm, ref sixbit);
                consumed = reader.Position;
                return true;
            }
            message = null;
            consumed = reader.Position;
            return false;

        }
    }
}