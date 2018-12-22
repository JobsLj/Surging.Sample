using Quartz;
using Surging.Core.Schedule.Configurations;

namespace Surging.Core.Schedule.Quartz
{
    public interface ISurgingTriggerFactory
    {
        ITrigger CreateTrigger(JobOption jobOption);
    }
}