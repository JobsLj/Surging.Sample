using System.Collections.Generic;

namespace Surging.Core.Schedule.Quartz.Runtime
{
    public interface IJobEntityProvider
    {
        IEnumerable<JobEntity> GetJobEntities();
    }
}