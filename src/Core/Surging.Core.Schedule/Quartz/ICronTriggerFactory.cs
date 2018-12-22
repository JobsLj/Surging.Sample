using Quartz;
using Surging.Core.CPlatform.Ioc;

namespace Surging.Core.Schedule.Quartz
{
    public interface ICronTriggerFactory : ITransientDependency
    {
        ITrigger CreateTrigger(string cronExpression);
    }
}