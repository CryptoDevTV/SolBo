using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services
{
    public interface ITickerPriceService
    {
        TickerPriceResponse GetPriceValue(AvailableStrategy availableStrategy);
    }
}