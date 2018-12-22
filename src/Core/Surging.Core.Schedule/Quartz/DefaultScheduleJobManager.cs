using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Surging.Core.Schedule.Quartz.Runtime;
using Surging.Core.Schedule.Utilities;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.Core.Schedule.Quartz
{
    public class DefaultScheduleJobManager : ISurgingScheduleJobManager
    {
        private readonly IScheduler _scheduler;

        public DefaultScheduleJobManager()
        {
            var quartzProps = GetQuartzProps();
            var factory = new StdSchedulerFactory(quartzProps);
            _scheduler = factory.GetScheduler().Result;
        }

        private NameValueCollection GetQuartzProps()
        {
            var props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

            if (AppConfig.IsClustered)
            {
                props["quartz.jobStore.clustered"] = "true";
                props["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
                props["quartz.jobStore.dataSource"] = AppConfig.ClusterOption.DataSource;
                props[$"quartz.dataSource.{ AppConfig.ClusterOption.DataSource}.connectionString"] = AppConfig.ClusterOption.DbConnectionString;
                //   props["quartz.scheduler.instanceName"] = "SurgingScheduler";
                props["quartz.scheduler.instanceId"] = "AUTO";
                props["durability"] = "true";
                props[$"quartz.dataSource.{AppConfig.ClusterOption.DataSource}.provider"] = AppConfig.ClusterOption.DataSourceProvider;
                switch (AppConfig.ClusterOption.DbType)
                {
                    case Configurations.QuartzClusterDbType.SqlServer:
                        props["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
                        break;

                    case Configurations.QuartzClusterDbType.MySql:
                        props["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";

                        break;

                    case Configurations.QuartzClusterDbType.Oracle:
                        props["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.OracleDelegate, Quartz";
                        break;
                }
            }

            return props;
        }

        public async Task ScheduleAsync(JobEntity jobEntity)
        {
            var jobToBuild = JobBuilder.Create(jobEntity.JobType)
                .WithIdentity(JobKey.Create(jobEntity.JobId, jobEntity.GroupName))
                .WithDescription(jobEntity.Describe);

            var job = jobToBuild.Build();
            var isExist = await _scheduler.CheckExists(job.Key);
            if (!isExist)
            {
                var jobOption = AppConfig.JobOptions.First(p => p.JobClass == jobEntity.JobClass);
                var trigger = jobEntity.TriggerFactory.CreateTrigger(jobOption);
                await _scheduler.ScheduleJob(job, trigger);
            }
        }

        public async Task Start()
        {
            await _scheduler.Start();
        }

        public Task Stop()
        {
            return _scheduler.Shutdown(true);
        }

        public Task ScheduleAsync(IJob job, ITrigger trigger, string jobId, string jobGroup = "default", string describe = "", IDictionary<string, object> jobData = null)
        {
            var jobToBuild = JobBuilder.Create(job.GetType())
               .WithIdentity(JobKey.Create(jobId, jobGroup))
               .WithDescription(describe);
            if (jobData != null)
            {
                jobToBuild.SetJobData(new JobDataMap(jobData));
            }

            var jobKey = jobToBuild.Build();

            return _scheduler.ScheduleJob(jobKey, trigger);
        }

        //public async Task ScheduleAsync(IDictionary<ISurgingJob,ITrigger> jobs)
        //{
        //    foreach (var job in jobs)
        //    {
        //       await ScheduleAsync(job.Key,job.Value);
        //    }
        //}

        public async Task PauseJob(string jobId, string groupName = null)
        {
            var jobkey = await GetJobKey(jobId, groupName);
            await _scheduler.PauseJob(jobkey);
        }

        public async Task RomoveJob(string jobId, string groupName = null)
        {
            var jobKey = await GetJobKey(jobId, groupName);
            await _scheduler.DeleteJob(jobKey);
        }

        public async Task ResumeJob(string jobId, string groupName = null)
        {
            var jobKey = await GetJobKey(jobId, groupName);
            await _scheduler.ResumeJob(jobKey);
        }

        protected async Task<JobKey> GetJobKey(string jobId, string groupName = null)
        {
            var matcher = GroupMatcher<JobKey>.AnyGroup();
            if (!string.IsNullOrEmpty(groupName))
            {
                matcher = GroupMatcher<JobKey>.GroupEquals(groupName);
            }

            var jobkey = (await _scheduler.GetJobKeys(matcher)).FirstOrDefault(p => p.Name == jobId);
            if (jobkey == null)
            {
                throw new QuartzException($"当前调度器中不存在Id为{jobId}的任务");
            }
            return jobkey;
        }
    }
}