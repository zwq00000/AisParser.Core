using System.Text;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;

namespace AisParser.Tests {
    public class SpanTests {

        [Fact]
        public async Task TestParseLinesAsync () {
            Console.WriteLine("Start");
            var client = new TcpClient ();
            client.Connect("153.44.253.27", 5631);
            Console.WriteLine("connect ...");
            await ProcessLinesAsync (client.Client);

        }

        private async Task ProcessLinesAsync (Socket socket) {
            var pipe = new Pipe ();
            Task writing = FillPipeAsync (socket, pipe.Writer);
            Task reading = ReadPipeAsync (pipe.Reader);

            await Task.WhenAll (reading, writing);
        }

        private async Task ReadPipeAsync (PipeReader reader) {
            while (true) {
                ReadResult result = await reader.ReadAsync ();

                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition? position = null;
            
                do {
                    if(TryParseLine(ref buffer,out var line)){
                        Console.WriteLine(Encoding.ASCII.GetString(line.ToArray()));
                     }
                }
                while (position != null);

                // Tell the PipeReader how much of the buffer we have consumed
                reader.AdvanceTo (buffer.Start, buffer.End);

                // Stop reading if there's no more data coming
                if (result.IsCompleted) {
                    break;
                }
            }

            // Mark the PipeReader as complete
            reader.Complete ();
        }

        async Task FillPipeAsync (Socket socket, PipeWriter writer) {
            const int minimumBufferSize = 512;

            while (true) {
                // Allocate at least 512 bytes from the PipeWriter
                Memory<byte> memory = writer.GetMemory (minimumBufferSize);
                try {
                    int bytesRead = await socket.ReceiveAsync (memory, SocketFlags.None);
                    if (bytesRead == 0) {
                        break;
                    }
                    // Tell the PipeWriter how much was read from the Socket
                    writer.Advance (bytesRead);
                } catch (Exception ex) {
                    LogError (ex);
                    break;
                }

                // Make the data available to the PipeReader
                FlushResult result = await writer.FlushAsync ();

                if (result.IsCompleted) {
                    break;
                }
            }

            // Tell the PipeReader that there's no more data coming
            writer.Complete ();
        }

        private void LogError (Exception ex) {
            //throw new NotImplementedException();
        }

        // public bool TryGetLine (ReadOnlySequence<byte> sequence, ) {
        //     var reader = new System.Buffers.SequenceReader<byte> (sequence);
        //     if (reader.TryAdvanceTo ((byte)
        //             '\n')) {
        //         reader.
        //     }
        // }

        static ReadOnlySpan<byte> NewLine => new byte[] {
            (byte)
            '\r', (byte)
            '\n'
        };

        static bool TryParseLine (ref ReadOnlySequence<byte> buffer,
            out ReadOnlySequence<byte> line) {
            var reader = new SequenceReader<byte> (buffer);

            if (reader.TryReadTo (out line, NewLine)) {
                buffer = buffer.Slice (reader.Position);
                return true;
            }

            line = default;
            return false;
        }
    }
}