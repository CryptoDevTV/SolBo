using FluentValidation;
using Solbo.Strategy.Beta.Models;

namespace Solbo.Strategy.Beta.Validators
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