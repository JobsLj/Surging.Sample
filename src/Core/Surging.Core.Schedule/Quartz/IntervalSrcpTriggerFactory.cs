using Quartz;
using Surging.Core.Schedule.Configurations;
using System;

namespace Surging.Core.Schedule.Quartz
{
    public class IntervalSurgingTriggerFactory : ISurgingTriggerFactory
    {
        public ITrigger CreateTrigger(JobOption jobOption)
        {
            var triggerBuilder = TriggerBuilder.Create();

            switch (jobOption.Unit)
            {
                case Configurations.IntervalUnit.Second:
                    triggerBuilder.WithSimpleSchedule(s =>
                    {
                        s.WithIntervalInSeconds(jobOption.IntervalTime)
                        .RepeatForever();
                    });
                    break;

                case Configurations.IntervalUnit.Minute:
                    triggerBuilder.WithSimpleSchedule(s =>
                    {
                        s.WithIntervalInMinutes(jobOption.IntervalTime)
                        .RepeatForever();
                    });
                    break;

                case Configurations.IntervalUnit.Hour:
                    triggerBuilder.WithSimpleSchedule(s =>
                    {
                        s.WithIntervalInHours(jobOption.IntervalTime)
                        .RepeatForever();
                    });
                    break;

                case Configurations.IntervalUnit.Day:
                    triggerBuilder.WithSimpleSchedule(s =>
                    {
                        s.WithIntervalInHours(jobOption.IntervalTime * 24)
                        .RepeatForever();
                    });
                    break;
            }

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
    }
}