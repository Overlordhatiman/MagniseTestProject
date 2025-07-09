using MagniseTask.Models;
using System.Collections.Concurrent;

namespace MagniseTask.Services.RealTime
{

    public class MarketDataService
    {
        private readonly WebSocketClientService _webSocketService;
        private readonly ConcurrentDictionary<string, PriceUpdate> _cache = new();

        public MarketDataService(WebSocketClientService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        public async Task StartAsync()
        {
            await _webSocketService.StartAsync(CancellationToken.None);

            var symbols = new[] { "EUR/USD", "GOOG" };
            await _webSocketService.SubscribeAsync(symbols);

            foreach (var symbol in symbols)
            {
                _webSocketService.RegisterCallback(symbol, update => _cache[symbol] = update);
            }
        }

        public PriceUpdate? GetPrice(string symbol)
        {
            return _cache.TryGetValue(symbol, out var value) ? value : null;
        }
    }
}
