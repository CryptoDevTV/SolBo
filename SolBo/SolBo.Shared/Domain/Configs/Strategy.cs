using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Domain.Configs
{
    public class Strategy
    {
        public int Interval { get; set; }
        public IntervalType IntervalType { get; set; }
        public string Name { get; set; }
    }
}