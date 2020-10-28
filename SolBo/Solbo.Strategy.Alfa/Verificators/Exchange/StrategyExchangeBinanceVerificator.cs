using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Validators.Exchange;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Verificators.Exchange
{
    internal class StrategyExchangeBinanceVerificator : IAlfaRule
    {
        public IRuleResult Result(StrategyRootModel strategyRootModel)
        {
            var validator = new StrategyExchangeBinanceValidator();
            var result = validator.Validate(strategyRootModel.Exchange.Binance);

            return new RuleResult(result.ToString());
        }
    }
}