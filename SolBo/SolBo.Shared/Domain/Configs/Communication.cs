using SolBo.Shared.Messages.Rules;

namespace SolBo.Shared.Domain.Configs
{
    public class Communication
    {
        public SymbolMessage Symbol { get; set; }
        public PriceMessage Price { get; set; }
        public PriceMessage Average { get; set; }
        public ChangeMessage StopLoss { get; set; }
        public ChangeMessage Buy { get; set; }
        public ChangeMessage Sell { get; set; }
        public AvailableAssetMessage AvailableAsset { get; set; }
    }
}