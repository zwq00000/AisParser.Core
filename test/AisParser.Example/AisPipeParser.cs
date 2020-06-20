using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AisParser.Example {
    internal class AisPipeParser {
        private readonly TcpClient _client;
        private readonly MessageParser _parser;
        private readonly CancellationTokenSource cancelSource;

        public AisPipeParser (TcpClient client) {
            this._client = client;
            this._parser = new MessageParser ();
            this.cancelSource = new CancellationTokenSource ();
        }

        public Task StartAsync (int count = 100) {
            var pipe = new Pipe ();
            var filltask = ReadAsync (_client, pipe.Writer,cancelSource.Token);
            var produceTask = Task.Run (async () => {
                for (int i = 0; i < count; i++) {
                    var msg = await ParseAsync (pipe.Reader, cancelSource.Token);
                    Console.WriteLine ($"{i} {msg}");
                }
                cancelSource.Cancel();
            });
            Task.WaitAll (filltask, produceTask);
            return Task.CompletedTask;
        }

        private async Task<Messages> ParseAsync (PipeReader reader, CancellationToken cancellationToken = default) {
            while (!cancellationToken.IsCancellationRequested) {
                ReadResult result = await reader.ReadAsync (cancellationToken);
                ReadOnlySequence<byte> buffer = result.Buffer;

                // In the event that no message is parsed successfully, mark consumed 
                // as nothing and examined as the entire buffer.
                SequencePosition consumed = buffer.Start;
                SequencePosition examined = buffer.End;

                try {
                    if (_parser.TryParse (ref buffer, out var message, out consumed)) {
                        return message;
                    }

                    // There's no more data to be processed.
                    if (result.IsCompleted) {
                        if (buffer.Length > 0) {
                            // The message is incomplete and there's no more data to process.
                            throw new InvalidDataException ("Incomplete message.");
                        }
                        break;
                    }
                } catch (Exception ex) {
                    Debug.WriteLine ("parse exception " + ex.Message);
                } finally {
                    reader.AdvanceTo (consumed);
                }
            }

            return null;
        }

        private async Task ReadAsync (TcpClient client, PipeWriter writer, CancellationToken cancellationToken = default) {
            int bufSize = 512;
            while (!cancellationToken.IsCancellationRequested) {
                var buf = writer.GetMemory (bufSize);
                try {
                    var read = await client.Client.ReceiveAsync (buf, SocketFlags.None, cancellationToken);
                    if (read == 0)
                        break;

                    writer.Advance (read);

                    Console.WriteLine ($"Read from stream {read} bytes");
                } catch {
                    break;
                }

                // Make the data available to the PipeReader
                var result = await writer.FlushAsync ();
                if (result.IsCanceled) {
                    break;
                }
            }
            await writer.CompleteAsync ();
        }
    }
}