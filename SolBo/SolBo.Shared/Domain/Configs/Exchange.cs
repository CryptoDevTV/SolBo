using SolBo.Shared.Domain.Enums;
using System.Text.Json.Serialization;

namespace SolBo.Shared.Domain.Configs
{
    public class Exchange
    {
        public ExchangeType? Type { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string PassPhrase { get; set; }
        [JsonIgnore]
        public bool IsInTestMode
        {
            get
            {
                if (Type == ExchangeType.Binance)
                    return string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiSecret);
                else if (Type == ExchangeType.KuCoin)
                    return string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiSecret) || string.IsNullOrWhiteSpace(PassPhrase);
                else
                    return true;
            }
        }
    }
}