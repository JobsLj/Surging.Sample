using System;

namespace Surging.Core.Schedule.Quartz.Runtime
{
    public class JobEntity
    {
        public string JobId
        {
            get
            {
                return JobType.FullName;
            }
        }

        public string JobClass { get; set; }

        public string GroupName { get; set; }

        public string Describe { get; set; }

        public Type JobType { get; set; }

        public ISurgingTriggerFactory TriggerFactory { get; set; }
    }
}