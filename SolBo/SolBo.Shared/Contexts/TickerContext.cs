using Binance.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using System.Threading.Tasks;

namespace SolBo.Shared.Contexts
{
    public class TickerContext
    {
        private IBinanceClient _binanceClient;

        public TickerContext(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }

        public async Task<TickerContextResponse> GetPriceValue(AvailableStrategy availableStrategy)
        {
            var result = new TickerContextResponse();

            if (availableStrategy.Ticker == 0)
            {
                var response = await _binanceClient.GetPriceAsync(availableStrategy.Symbol);

                if (response.Success)
                {
                    result.SetResult(response.Data.Price);
                }
                else
                {
                    result.Message = response.Error.Message;
                }
            }
            else if (availableStrategy.Ticker == 1)
            {
                var response = await _binanceClient.GetCurrentAvgPriceAsync(availableStrategy.Symbol);

                if (response.Success)
                {
                    result.SetResult(response.Data.Price);
                }
                else
                {
                    result.Message = response.Error.Message;
                }
            }
            else
            {
                result.Message = "Bad ticker definition";
            }
            return result;
        }
    }

    public class TickerContextResponse
    {
        public decimal Result { get; private set; }
        public void SetResult(decimal result)
        {
            Result = decimal.Round(result, 2);
        }
        public string Message { get; set; }
        public bool Success => string.IsNullOrWhiteSpace(Message);
    }
}
