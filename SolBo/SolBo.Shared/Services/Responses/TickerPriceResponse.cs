namespace SolBo.Shared.Services.Responses
{
    public class TickerPriceResponse
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