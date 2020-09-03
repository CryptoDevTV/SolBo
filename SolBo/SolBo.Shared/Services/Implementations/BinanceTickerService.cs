using Binance.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services.Implementations
{
    public class BinanceTickerService : IBinanceTickerService
    {
        private readonly IBinanceClient _binanceClient;
        public BinanceTickerService(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public TickerPriceResponse GetPriceValue(AvailableStrategy availableStrategy)
        {
            var result = new TickerPriceResponse();

            var response = _binanceClient.GetPrice(availableStrategy.Symbol);

            if (response.Success)
            {
                result.SetResult(response.Data.Price);
            }
            else
            {
                result.Message = response.Error.Message;
            }

            return result;
        }
    }
}