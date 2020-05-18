using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace SolBo.Agent.Factories
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScope _scope;
        public JobFactory(IServiceProvider container)
        {
            _scope = container.CreateScope();
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var res = _scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return res;
        }
        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}