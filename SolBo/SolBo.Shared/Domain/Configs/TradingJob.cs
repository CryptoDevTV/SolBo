using SolBo.Shared.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SolBo.Shared.Domain.Configs
{
    public class TradingJob
    {
        public int ActiveId { get; set; }
        public ModeType ModeType { get; set; }
        public IEnumerable<TradingJobAvailable> Available { get; set; }
        [JsonIgnore]
        public TradingJobAvailable AvailableStrategy
            => Available.FirstOrDefault(s => s.Id == ActiveId);
    }
}