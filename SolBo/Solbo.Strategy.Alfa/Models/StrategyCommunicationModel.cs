using Binance.Net.Objects.Spot.MarketData;

namespace Solbo.Strategy.Alfa.Models
{
    public class StrategyCommunicationModel
    {
        public decimal? CurrentPrice { get; set; }
        public BinanceSymbol BinanceSymbol { get; set; }
        public decimal CurrentAverage { get; set; }
    }
}