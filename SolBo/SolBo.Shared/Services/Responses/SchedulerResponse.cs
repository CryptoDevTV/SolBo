using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Services.Responses
{
    public class SchedulerResponse
    {
        public Solbot SolBotConfig { get; set; }
        public bool WriteSuccess { get; set; }
        public bool ReadSucces => !(SolBotConfig is null);
    }
}