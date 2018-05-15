using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Programmer.WebServer
{
    public class WebSocketOutlet
    {
        private static readonly List<byte> Buffer = new List<byte>();

        public static async Task Send(WebSocket webSocket)
        {
            while (!webSocket.CloseStatus.HasValue)
            {
                if (!Buffer.Any())
                {
                    Thread.Sleep(150);
                    continue;
                }

                await webSocket.SendAsync(
                    new ArraySegment<byte>(Buffer.ToArray(), 0, Buffer.Count), 
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
                Buffer.Clear();
            }
        }

        public static void AddToBuffer(string data)
        {
            Buffer.AddRange(Encoding.UTF8.GetBytes(data));
        }
    }
}
