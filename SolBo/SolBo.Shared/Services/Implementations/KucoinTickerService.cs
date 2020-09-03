using Kucoin.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services.Implementations
{
    public class KucoinTickerService : IKucoinTickerService
    {
        private readonly IKucoinClient _kucoinClient;
        public KucoinTickerService(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public TickerPriceResponse GetPriceValue(AvailableStrategy availableStrategy)
        {
            var result = new TickerPriceResponse();

            var response = _kucoinClient.GetTicker(availableStrategy.Symbol);

            if (response.Success)
            {
                result.SetResult(response.Data.LastTradePrice.GetValueOrDefault());
            }
            else
            {
                result.Message = response.Error.Message;
            }

            return result;
        }
    }
}