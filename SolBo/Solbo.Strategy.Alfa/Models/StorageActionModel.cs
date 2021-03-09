using System.Text.Json.Serialization;

namespace Solbo.Strategy.Alfa.Models
{
    public class StorageActionModel
    {
        public decimal BoughtPrice { get; set; }
        public bool StopLossReached { get; set; }
        public int StopLossCurrentCycle { get; set; }
        [JsonIgnore]
        public bool BoughtBefore
            => BoughtPrice > 0;
        [JsonIgnore]
        public bool SellBefore
            => BoughtPrice == 0;
    }
}