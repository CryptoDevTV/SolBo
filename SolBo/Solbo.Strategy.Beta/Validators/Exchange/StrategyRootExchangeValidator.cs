using FluentValidation;
using Solbo.Strategy.Beta.Models;

namespace Solbo.Strategy.Beta.Validators.Exchange
{
    public class StrategyRootExchangeValidator : AbstractValidator<StrategyRootExchange>
    {
        public StrategyRootExchangeValidator()
        {
            RuleFor(ex => ex.Kucoin).NotNull();
            RuleFor(ex => ex.Kucoin.ExchangeType).Equal(ex => ex.ActiveExchangeType);
        }
    }
}