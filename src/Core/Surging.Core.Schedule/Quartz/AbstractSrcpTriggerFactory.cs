using Quartz;
using Surging.Core.Schedule.Configurations;
using System;

namespace Surging.Core.Schedule.Quartz
{
    public abstract class AbstractSurgingTriggerFactory : ISurgingTriggerFactory
    {
        public ITrigger CreateTrigger(JobOption jobOption)
        {
            var triggerBuilder = CreateTriggerBuilder();

            if (jobOption.DelayTime == 0)
            {
                triggerBuilder = triggerBuilder.StartNow();
            }
            else
            {
                var startTime = DateTime.UtcNow.AddMilliseconds(jobOption.DelayTime);
                triggerBuilder = triggerBuilder.StartAt(startTime);
            }

            return triggerBuilder.Build();
        }

        public abstract TriggerBuilder CreateTriggerBuilder();
    }
}