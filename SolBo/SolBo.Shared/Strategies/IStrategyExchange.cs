using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Strategies
{
    public interface IStrategyExchange
    {
        ExchangeType ActiveExchangeType { get; set; }
    }
}