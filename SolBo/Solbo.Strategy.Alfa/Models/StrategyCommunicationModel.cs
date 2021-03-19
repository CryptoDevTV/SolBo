using Binance.Net.Objects.Spot.MarketData;

namespace Solbo.Strategy.Alfa.Models
{
    public class StrategyCommunicationModel
    {
        public decimal? CurrentPrice { get; set; }
        public BinanceSymbol BinanceSymbol { get; set; }
        public decimal CurrentAverage { get; set; }
        public decimal BaseAsset { get; set; }
        public decimal QuoteAsset { get; set; }
        public bool IsPossibleStopLoss { get; set; }
        public bool IsPossibleSell { get; set; }
        public bool IsPossibleBuy { get; set; }
    }
}