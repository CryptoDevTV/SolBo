using FluentValidation;
using Solbo.Strategy.Alfa.Models;

namespace Solbo.Strategy.Alfa.Validators.Exchange
{
    public class ExchangeValidator : AbstractValidator<StrategyRootExchange>
    {
        public ExchangeValidator()
        {
            RuleFor(ex => ex.Binance).NotNull();
            RuleFor(ex => ex.Binance.ExchangeType).Equal(ex => ex.ActiveExchangeType);
        }
    }
}