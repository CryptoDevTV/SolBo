using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Domain.Configs
{
    public class Pair
    {
        public string Symbol { get; set; }
        public IntervalType IntervalType { get; set; }
        public int Interval { get; set; }
    }
}