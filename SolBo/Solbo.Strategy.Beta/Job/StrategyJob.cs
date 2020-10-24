using Quartz;
using Solbo.Strategy.Beta.Models;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Solbo.Strategy.Beta.Job
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
            var jobArgs = await _fileService.GetAsync<StrategyRootModel>(path);

            var symbol = context.JobDetail.JobDataMap["symbol"] as string;

            var jobPerSymbol = jobArgs.Pairs.FirstOrDefault(j => j.Symbol == symbol);

            _loggingService.Info($"" +
                $"{jobPerSymbol.Text} - " +
                $"{jobPerSymbol.Symbol} - " +
                $"{jobArgs.Exchange.ActiveExchangeType.GetDescription()} - " +
                $"{jobArgs.Exchange.Kucoin}");
        }
    }
}