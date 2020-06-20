using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;

namespace AisParser.Example
{
    class Program {
        static void Main (string[] args) {
            Console.WriteLine("start ais parse...");
            // using (var reader = CreateReader()) {
            //     for(int i=0;i<100;i++){
            //         Console.WriteLine(reader.ReadLine());
            //     }
            // }
            //StartParse (GetLines (CreateReader ()).Take (1000));
            //StartAnalysis (GetLines (CreateReader ()));
            TcpClient client = GetClient ();
            var parser = new AisPipeParser(client);

            parser.StartAsync(1000).Wait();
        }

        private static TcpClient GetClient () {
            //挪威 开放AIS数据 https://kystverket.no/Maritime-tjenester/Meldings--og-informasjonstjenester/AIS/Brukartilgang-til-AIS-Norge/
            var client = new TcpClient ();
            client.Connect ("153.44.253.27", 5631);
            //client.Connect("localhost", 9050);
            //client.Connect ("192.168.1.106", 8040);
            Console.WriteLine($"open {client.Client.RemoteEndPoint}");
            return client;
        }

        #region  stream reader

        private static StreamReader CreateReader () {
            //挪威 开放AIS数据 https://kystverket.no/Maritime-tjenester/Meldings--og-informasjonstjenester/AIS/Brukartilgang-til-AIS-Norge/
            var client = new TcpClient ();
            client.Connect ("153.44.253.27", 5631);
            //client.Connect("localhost", 9050);
            //client.Connect ("192.168.1.106", 8040);
            return new StreamReader (client.GetStream ());
        }

        #endregion

    }

    public interface IVdmParser {
        /// <summary>
        /// 尝试从<see ref="ReadOnlySequence{byte}"></see>中读取数据包
        /// </summary>
        /// <param name="buffer">消息流</param>
        /// <param name="vdm">解析的消息</param>
        /// <param name="consumed">消费字节数量</param>
        /// <returns></returns>
        bool TryParsePackage (ref ReadOnlySequence<byte> buffer, out VdmVdo vdm, out SequencePosition consumed);

    }
}