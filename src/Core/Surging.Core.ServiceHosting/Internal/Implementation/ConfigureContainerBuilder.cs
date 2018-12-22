using System;
using System.Reflection;

namespace Surging.Core.ServiceHosting.Internal.Implementation
{
    /// <summary>
    /// Startup Configure方法构造器
    /// </summary>
    public class ConfigureContainerBuilder
    {
        public ConfigureContainerBuilder(MethodInfo configureContainerMethod)
        {
            MethodInfo = configureContainerMethod;
        }

        public MethodInfo MethodInfo { get; }

        public Action<object> Build(object instance) => container => Invoke(instance, container);

        public Type GetContainerType()
        {
            var parameters = MethodInfo.GetParameters();
            if (parameters.Length != 1)
            {
                throw new InvalidOperationException($"{MethodInfo.Name} 方法必须有一个参数");
            }
            return parameters[0].ParameterType;
        }

        private void Invoke(object instance, object container)
        {
            if (MethodInfo == null)
            {
                return;
            }
            var arguments = new object[1] { container };
            MethodInfo.Invoke(instance, arguments);
        }
    }
}