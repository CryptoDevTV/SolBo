using FluentValidation;
using Solbo.Strategy.Alfa.Models;

namespace Solbo.Strategy.Alfa.Validators
{
    public class StrategyRootExchangeValidator : AbstractValidator<StrategyRootExchange>
    {
        public StrategyRootExchangeValidator()
        {
            RuleFor(ex => ex.Binance).NotNull();
            RuleFor(ex => ex.Binance.ExchangeType).Equal(ex => ex.ActiveExchangeType);
        }
    }
}