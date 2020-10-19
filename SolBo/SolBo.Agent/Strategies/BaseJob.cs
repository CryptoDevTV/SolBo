using SolBo.Shared.Domain.Enums;

namespace SolBo.Agent.Strategies
{
    public abstract class BaseJob
    {
        public int Id { get; set; }
        public ModeType ModeType { get; set; }
        public int IntervalInMinutes { get; set; }
        public string Symbol { get; set; }

        public bool IsActive => IntervalInMinutes > 0;
    }
}