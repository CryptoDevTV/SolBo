using System.Collections.Generic;

namespace SolBo.Shared.Domain.Configs
{
    public class Strategy
    {
        public int ActiveId { get; set; }
        public int IntervalInMinutes { get; set; }
        public IEnumerable<AvailableStrategy> Available { get; set; }
    }
}