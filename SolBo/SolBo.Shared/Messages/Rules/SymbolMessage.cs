namespace SolBo.Shared.Messages.Rules
{
    public class SymbolMessage
    {
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public int QuoteAssetPrecision { get; set; }
        public decimal MinNotional { get; set; }
        public decimal StepSize { get; set; }
        public decimal MaxQuantity { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal TickSize { get; set; }
    }
}