using FluentValidation;
using Solbo.Strategy.Beta.Models;

namespace Solbo.Strategy.Beta.Validators.Strategy
{
    public class StrategyModelValidator : AbstractValidator<StrategyModel>
    {
        public StrategyModelValidator()
        {
            RuleFor(m => m.Symbol).NotEmpty();
            RuleFor(m => m.Text).NotEmpty();
        }
    }
}