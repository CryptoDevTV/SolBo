using SolBo.Shared.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SolBo.Shared.Domain.Configs
{
    public class Strategy
    {
        public int ActiveId { get; set; }
        public ModeType ModeType { get; set; }
        public IEnumerable<AvailableStrategy> Available { get; set; }
        [JsonIgnore]
        public AvailableStrategy AvailableStrategy
            => Available.FirstOrDefault(s => s.Id == ActiveId);
    }
}