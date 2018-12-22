using Autofac;
using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Module;
using Surging.Core.Schedule.Configurations;
using Surging.Core.Schedule.Quartz;
using Surging.Core.Schedule.Quartz.Runtime;
using Surging.Core.Schedule.Quartz.Runtime.Implementation;
using System.Collections.Generic;

namespace Surging.Core.Schedule
{
    public class ScheduleModule : EnginePartModule
    {
        public override void Initialize(CPlatformContainer serviceProvider)
        {
            base.Initialize(serviceProvider);
            var jobEntities = serviceProvider.GetInstances<IJobEntityProvider>().GetJobEntities();
            var srcpScheduleJobManager = serviceProvider.GetInstances<ISurgingScheduleJobManager>();
            foreach (var jobEntity in jobEntities)
            {
                srcpScheduleJobManager.ScheduleAsync(jobEntity).Wait();
            }
            if (AppConfig.ImmediateStart)
            {
                srcpScheduleJobManager.Start().Wait();
            }
        }

        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
            var jobsSection = AppConfig.GetSection("jobs");
            //if (!jobsSection.Exists())
            //{
            //    throw new CPlatformException("不存在任务配置");
            //}

            var jobs = new List<JobOption>();
            AppConfig.JobOptions = jobsSection.Get<ICollection<JobOption>>();

            var isClusterSection = AppConfig.GetSection("isClustered");
            if (!isClusterSection.Exists())
            {
                AppConfig.IsClustered = false;
            }
            else
            {
                AppConfig.IsClustered = isClusterSection.Get<bool>();
                if (AppConfig.IsClustered)
                {
                    var clusterOptionSection = AppConfig.GetSection("clusterOption");
                    AppConfig.ClusterOption = clusterOptionSection.Get<ClusterOption>();
                }
            }
            var immediateStartSection = AppConfig.GetSection("immediateStart");
            if (!immediateStartSection.Exists())
            {
                AppConfig.ImmediateStart = false;
            }
            else
            {
                AppConfig.ImmediateStart = immediateStartSection.Get<bool>();
            }

            base.RegisterBuilder(builder);

            builder.RegisterType<JobEntityProvider>().As<IJobEntityProvider>();
            builder.RegisterType<DefaultScheduleJobManager>().As<ISurgingScheduleJobManager>().SingleInstance();
            builder.RegisterType<CronSurgingTriggerFactory>().As<ICronTriggerFactory>().InstancePerDependency();
        }
    }
}