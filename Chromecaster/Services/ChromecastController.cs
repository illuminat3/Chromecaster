using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;
using Chromecaster.Datatypes;
using System.Collections.Generic;

namespace Chromecaster.Services 
{ 
    public class ChromecastController
    {
        private readonly Dictionary<string, ClientWebSocket> _webSockets;

        public ChromecastController()
        {
            _webSockets = new Dictionary<string, ClientWebSocket>();
        }

        public async Task ConnectAsync(ChromecastDevice device)
        {
            string ipAddress = device.IPAddress;
            var webSocket = new ClientWebSocket();
            var chromecastUri = new Uri($"ws://{ipAddress}:8009");
            await webSocket.ConnectAsync(chromecastUri, CancellationToken.None);

            _webSockets[ipAddress] = webSocket;
        }

        public async Task SendMessageAsync(ChromecastDevice device, Message message)
        {
            string ipAddress = device.IPAddress;

            if (_webSockets.ContainsKey(ipAddress))
            {
                var webSocket = _webSockets[ipAddress];
                string messageString = JsonSerializer.Serialize(message);
                byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);

                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                throw new InvalidOperationException("WebSocket connection not found for this IP address. Please Connect First");
            }
        }

        public async Task DisconnectAsync(string ipAddress)
        {
            if (_webSockets.ContainsKey(ipAddress))
            {
                var webSocket = _webSockets[ipAddress];
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", CancellationToken.None);

                _webSockets.Remove(ipAddress);
            }
        }

        public async Task DisconnectAllAsync()
        {
            foreach (var webSocket in _webSockets.Values)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", CancellationToken.None);
            }

            _webSockets.Clear();
        }
    } 
}