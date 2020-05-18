using SolBo.Shared.Messages.Rules;

namespace SolBo.Shared.Domain.Configs
{
    public class Communication
    {
        public SymbolMessage Symbol { get; set; }
        public PriceMessage Price { get; set; }
        public PriceMessage Average { get; set; }
        public PercentageMessage StopLoss { get; set; }
        public PercentageMessage Buy { get; set; }
        public PercentageMessage Sell { get; set; }
    }
}