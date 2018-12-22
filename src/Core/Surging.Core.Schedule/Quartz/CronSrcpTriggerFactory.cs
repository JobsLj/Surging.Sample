using Quartz;
using Surging.Core.Schedule.Configurations;
using Surging.Core.Schedule.Utilities;
using System;
using System.Text.RegularExpressions;

namespace Surging.Core.Schedule.Quartz
{
    public class CronSurgingTriggerFactory : ISurgingTriggerFactory, ICronTriggerFactory
    {
        private const string cornRegEx = "(((^([0-9]|[0-5][0-9])(\\,|\\-|\\/){1}([0-9]|[0-5][0-9]) )|^([0-9]|[0-5][0-9]) |^(\\* ))((([0-9]|[0-5][0-9])(\\,|\\-|\\/){1}([0-9]|[0-5][0-9]) )|([0-9]|[0-5][0-9]) |(\\* ))((([0-9]|[01][0-9]|2[0-3])(\\,|\\-|\\/){1}([0-9]|[01][0-9]|2[0-3]) )|([0-9]|[01][0-9]|2[0-3]) |(\\* ))((([0-9]|[0-2][0-9]|3[01])(\\,|\\-|\\/){1}([0-9]|[0-2][0-9]|3[01]) )|(([0-9]|[0-2][0-9]|3[01]) )|(\\? )|(\\* )|(([1-9]|[0-2][0-9]|3[01])L )|([1-7]W )|(LW )|([1-7]\\#[1-4] ))((([1-9]|0[1-9]|1[0-2])(\\,|\\-|\\/){1}([1-9]|0[1-9]|1[0-2]) )|([1-9]|0[1-9]|1[0-2]) |(\\* ))(([1-7](\\,|\\-|\\/){1}[1-7])|([1-7])|(\\?)|(\\*)|(([1-7]L)|([1-7]\\#[1-4]))))|(((^([0-9]|[0-5][0-9])(\\,|\\-|\\/){1}([0-9]|[0-5][0-9]) )|^([0-9]|[0-5][0-9]) |^(\\* ))((([0-9]|[0-5][0-9])(\\,|\\-|\\/){1}([0-9]|[0-5][0-9]) )|([0-9]|[0-5][0-9]) |(\\* ))((([0-9]|[01][0-9]|2[0-3])(\\,|\\-|\\/){1}([0-9]|[01][0-9]|2[0-3]) )|([0-9]|[01][0-9]|2[0-3]) |(\\* ))((([0-9]|[0-2][0-9]|3[01])(\\,|\\-|\\/){1}([0-9]|[0-2][0-9]|3[01]) )|(([0-9]|[0-2][0-9]|3[01]) )|(\\? )|(\\* )|(([1-9]|[0-2][0-9]|3[01])L )|([1-7]W )|(LW )|([1-7]\\#[1-4] ))((([1-9]|0[1-9]|1[0-2])(\\,|\\-|\\/){1}([1-9]|0[1-9]|1[0-2]) )|([1-9]|0[1-9]|1[0-2]) |(\\* ))(([1-7](\\,|\\-|\\/){1}[1-7] )|([1-7] )|(\\? )|(\\* )|(([1-7]L )|([1-7]\\#[1-4]) ))((19[789][0-9]|20[0-9][0-9])\\-(19[789][0-9]|20[0-9][0-9])))";

        public ITrigger CreateTrigger(JobOption jobOption)
        {
            if (!Regex.IsMatch(jobOption.CronExpression, cornRegEx))
            {
                throw new QuartzException("Corn表达式错误");
            }
            var triggerBuilder = TriggerBuilder.Create()
               .WithCronSchedule(jobOption.CronExpression);

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

        public ITrigger CreateTrigger(string cronExpression)
        {
            var triggerBuilder = TriggerBuilder.Create()
              .WithCronSchedule(cronExpression);
            return triggerBuilder.Build();
        }
    }
}