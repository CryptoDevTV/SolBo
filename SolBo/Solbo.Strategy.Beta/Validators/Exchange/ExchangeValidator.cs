using FluentValidation;
using Solbo.Strategy.Beta.Models;

namespace Solbo.Strategy.Beta.Validators.Exchange
{
    public class ExchangeValidator : AbstractValidator<StrategyRootExchange>
    {
        public ExchangeValidator()
        {
            RuleFor(ex => ex.Kucoin).NotNull();
            RuleFor(ex => ex.Kucoin.ExchangeType).Equal(ex => ex.ActiveExchangeType);
        }
    }
}