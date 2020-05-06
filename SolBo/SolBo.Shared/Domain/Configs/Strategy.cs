using System.Collections.Generic;

namespace SolBo.Shared.Domain.Configs
{
    public class Strategy
    {
        public int ActiveId { get; set; }
        public int IntervalInMinutes { get; set; }
        public int TestMode { get; set; }
        public IEnumerable<AvailableStrategy> Available { get; set; }

        public bool IsNotInTestMode => TestMode == 0;
    }
}