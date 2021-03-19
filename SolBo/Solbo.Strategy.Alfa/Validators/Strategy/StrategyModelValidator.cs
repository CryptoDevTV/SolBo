using FluentValidation;
using Solbo.Strategy.Alfa.Models;

namespace Solbo.Strategy.Alfa.Validators.Strategy
{
    public class StrategyModelValidator : AbstractValidator<StrategyModel>
    {
        public StrategyModelValidator()
        {
            RuleFor(m => m.Symbol).NotEmpty();
            RuleFor(m => m.BuyDown).NotEmpty().GreaterThan(0);
            RuleFor(m => m.SellUp).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Average).NotEmpty().GreaterThan(0);
            RuleFor(m => m.StopLossDown).GreaterThanOrEqualTo(0);
            RuleFor(m => m.FundPercentage).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100);
            RuleFor(m => m.ClearOnStartup).Must(m => m == true || m == false);
            RuleFor(m => m.StopLossPauseCycles).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(m => m.AverageType).IsInEnum();
            RuleFor(m => m.SellType).IsInEnum();
            RuleFor(m => m.CommissionType).IsInEnum();
        }
    }
}