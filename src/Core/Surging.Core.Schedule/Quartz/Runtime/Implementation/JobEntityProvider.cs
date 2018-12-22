using Quartz;
using Surging.Core.Schedule.Utilities;
using System;
using System.Collections.Generic;

namespace Surging.Core.Schedule.Quartz.Runtime.Implementation
{
    public class JobEntityProvider : IJobEntityProvider
    {
        public IEnumerable<JobEntity> GetJobEntities()
        {
            var jobEntities = new List<JobEntity>();

            if (AppConfig.JobOptions == null)
            {
                return jobEntities;
            }

            foreach (var option in AppConfig.JobOptions)
            {
                var jobType = Type.GetType(option.JobClass, throwOnError: true);
                if (!typeof(IJob).IsAssignableFrom(jobType))
                {
                    throw new QuartzException($"任务{option.JobClass}未继承自IJob接口");
                }

                Type triggerFactoryType = null;
                switch (option.ScheduleType)
                {
                    case Configurations.ScheduleType.UserDefined:
                        if (string.IsNullOrEmpty(option.TriggerFactoryClass))
                        {
                            throw new QuartzException("自定义任务调度触发器配置不允许为null");
                        }
                        triggerFactoryType = Type.GetType(option.TriggerFactoryClass, throwOnError: true);
                        if (!typeof(ISurgingTriggerFactory).IsAssignableFrom(triggerFactoryType))
                        {
                            throw new QuartzException($"任务触发器工厂{option.TriggerFactoryClass}未继承自ISurgingTriggerFactory接口");
                        }
                        break;

                    case Configurations.ScheduleType.Interval:
                        if (!option.Unit.HasValue || option.IntervalTime <= 0)
                        {
                            throw new QuartzException("定时任务调度器配置不正确");
                        }
                        triggerFactoryType = typeof(IntervalSurgingTriggerFactory);
                        break;

                    case Configurations.ScheduleType.Cron:
                        if (string.IsNullOrEmpty(option.CronExpression))
                        {
                            throw new QuartzException("任务调度触发器配置不允许为null");
                        }
                        triggerFactoryType = typeof(CronSurgingTriggerFactory);
                        break;
                }

                var jobEntity = new JobEntity()
                {
                    JobClass = option.JobClass,
                    GroupName = option.GroupName,
                    Describe = option.JobDescribe,
                    JobType = jobType,
                    TriggerFactory = (ISurgingTriggerFactory)Activator.CreateInstance(triggerFactoryType)
                };
                jobEntities.Add(jobEntity);
            }
            return jobEntities;
        }
    }
}