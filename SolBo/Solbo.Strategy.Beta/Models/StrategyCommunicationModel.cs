using Kucoin.Net.Objects;

namespace Solbo.Strategy.Beta.Models
{
    public class StrategyCommunicationModel
    {
        public decimal? CurrentPrice { get; set; }
        public KucoinSymbol KucoinSymbol { get; set; }
        public decimal CurrentAverage { get; set; }
    }
}