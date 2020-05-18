using Binance.Net.Interfaces;
using SolBo.Shared.Domain.Configs;

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

        public TickerContextResponse GetPriceValue(AvailableStrategy availableStrategy)
        {
            var result = new TickerContextResponse();

            if (availableStrategy.Ticker == 0)
            {
                var response = _binanceClient.GetPrice(availableStrategy.Symbol);

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
                var response = _binanceClient.GetCurrentAvgPrice(availableStrategy.Symbol);

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
            Result = result;
        }
        public string Message { get; set; }
        public bool Success => string.IsNullOrWhiteSpace(Message);
    }
}