using FluentValidation;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Strategies.Predefined.Exchanges;

namespace Solbo.Strategy.Alfa.Validators
{
    public class StrategyExchangeBinanceValidator : AbstractValidator<StrategyExchangeBinance>
    {
        public StrategyExchangeBinanceValidator()
        {
            RuleFor(ex => ex.ApiKey).NotEmpty();
            RuleFor(ex => ex.ApiSecret).NotEmpty();
            RuleFor(ex => ex.ExchangeType).Equal(ExchangeType.Binance);
        }
    }
}