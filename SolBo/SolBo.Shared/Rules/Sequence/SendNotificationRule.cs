using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class SendNotificationRule : ISequencedRule
    {
        public string SequenceName => "SendNotification";
        private readonly IPushOverNotificationService _pushOverNotificationService;
        private readonly bool _runForTheFirstTime = false;
        public SendNotificationRule(
            IPushOverNotificationService pushOverNotificationService,
            DateTimeOffset? runLastTime)
        {
            _pushOverNotificationService = pushOverNotificationService;
            _runForTheFirstTime = runLastTime is null;
        }

        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult
            {
                Success = true,
                Message = LogGenerator.SequenceSuccess(SequenceName, $"Notification ({_pushOverNotificationService.IsActive()})")
            };
            try
            {
                if (_runForTheFirstTime)
                {
                    _pushOverNotificationService.Send(
                        LogGenerator.NotificationTitleStart,
                        LogGenerator.NotificationMessageStart);
                }
            }
            catch (Exception e)
            {
                result.Message = LogGenerator.SequenceException(SequenceName, e);
            }

            return result;
        }
    }
}