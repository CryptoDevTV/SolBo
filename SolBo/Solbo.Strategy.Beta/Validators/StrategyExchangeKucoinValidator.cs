using FluentValidation;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Strategies.Predefined.Exchanges;

namespace Solbo.Strategy.Beta.Validators
{
    public class StrategyExchangeKucoinValidator : AbstractValidator<StrategyExchangeKucoin>
    {
        public StrategyExchangeKucoinValidator()
        {
            RuleFor(ex => ex.ApiKey).NotEmpty();
            RuleFor(ex => ex.ApiSecret).NotEmpty();
            RuleFor(ex => ex.PassPhrase).NotEmpty();
            RuleFor(ex => ex.ExchangeType).Equal(ExchangeType.KuCoin);
        }
    }
}