namespace Surging.Core.Schedule.Configurations
{
    public class JobOption
    {
        public string JobClass { get; set; }

        public string GroupName { get; set; }

        public string JobDescribe { get; set; }

        public ScheduleType ScheduleType { get; set; }

        public string CronExpression { get; set; }

        public IntervalUnit? Unit { get; set; }

        public int IntervalTime { get; set; }

        public int DelayTime { get; set; } = 0;

        public string TriggerFactoryClass { get; set; }
    }
}