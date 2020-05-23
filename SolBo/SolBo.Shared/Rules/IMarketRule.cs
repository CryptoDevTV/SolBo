using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Rules
{
    public interface IMarketRule : IRule
    {
        MarketOrderType MarketOrder { get; }
    }
}