namespace Solbo.Strategy.Beta.Models
{
    public class StorageActionModel
    {
        public decimal BoughtPrice { get; set; }
        public bool StopLossReached { get; set; }
        public int StopLossCurrentCycle { get; set; }
    }
}