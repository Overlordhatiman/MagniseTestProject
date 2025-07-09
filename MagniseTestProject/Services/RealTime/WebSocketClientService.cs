using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using MagniseTask.Models;
using Microsoft.Extensions.Logging;

namespace MagniseTask.Services.RealTime
{

    public class WebSocketClientService : IHostedService
    {
        private readonly ILogger<WebSocketClientService> _logger;
        private readonly Uri _webSocketUri;
        private readonly ClientWebSocket _client = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Dictionary<string, Action<PriceUpdate>> _subscribers = new();

        public WebSocketClientService(IConfiguration config, ILogger<WebSocketClientService> logger)
        {
            _logger = logger;
            _webSocketUri = new Uri(config["FintachartsAPI:WSS_Uri"]!);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _client.ConnectAsync(_webSocketUri, cancellationToken);
                _ = Task.Run(ReceiveLoop);
                _logger.LogInformation("WebSocket connected.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WebSocket connection failed.");
            }
        }

        public async Task SubscribeAsync(string[] symbols)
        {
            var message = JsonSerializer.Serialize(new { action = "subscribe", symbols });
            var buffer = Encoding.UTF8.GetBytes(message);
            if (_client.State == WebSocketState.Open)
            {
                await _client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                _logger.LogWarning("WebSocket is not open. Current state: {State}", _client.State);
            }

            _logger.LogInformation("Subscribed to symbols: {Symbols}", string.Join(", ", symbols));
        }

        public void RegisterCallback(string symbol, Action<PriceUpdate> callback)
        {
            _subscribers[symbol] = callback;
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[4096];

            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                    if (result.MessageType == WebSocketMessageType.Close) break;

                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var update = JsonSerializer.Deserialize<PriceUpdate>(json);

                    if (update != null && _subscribers.TryGetValue(update.Symbol, out var callback))
                        callback(update);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error receiving WebSocket message");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            if (_client.State == WebSocketState.Open)
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown", CancellationToken.None);
        }
    }
}
