using System.Text.Json.Serialization;

namespace SolBo.Shared.Domain.Configs
{
    public class Actions
    {
        public decimal BoughtPrice { get; set; }
        public int StopLossCurrentCycle { get; set; }
        public bool StopLossReached { get; set; }

        [JsonIgnore]
        public bool BoughtBefore
            => BoughtPrice > 0;
        [JsonIgnore]
        public bool SellBefore
            => BoughtPrice == 0;
    }
}