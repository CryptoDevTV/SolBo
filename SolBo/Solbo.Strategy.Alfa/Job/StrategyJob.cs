using Quartz;
using Solbo.Strategy.Alfa.Models;
using SolBo.Shared.Services;
using System.Threading.Tasks;

namespace Solbo.Strategy.Alfa.Job
{
    public class StrategyJob : IJob
    {
        private readonly IFileService _fileService;
        private readonly ILoggingService _loggingService;
        public StrategyJob(
            IFileService fileService,
            ILoggingService loggingService)
        {
            _fileService = fileService;
            _loggingService = loggingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var path = context.JobDetail.JobDataMap["path"] as string;
            var jobArgs = await _fileService.GetAsync<StrategyModel>(path);

            _loggingService.Info($"{jobArgs.Header}");
        }
    }
}