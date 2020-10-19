using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services
{
    public interface ITickerService
    {
        TickerPriceResponse GetPriceValue(TradingJobAvailable availableStrategy);
    }
}