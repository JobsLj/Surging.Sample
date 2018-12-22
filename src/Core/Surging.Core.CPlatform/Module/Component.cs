using System.Text;

namespace Surging.Core.CPlatform.Module
{
    #region 组件生命周期枚举类

    public enum LifetimeScope
    {
        InstancePerDependency,

        InstancePerHttpRequest,

        SingleInstance
    }

    #endregion 组件生命周期枚举类

    #region 组件类

    public class Component
    {
        #region 实例属性

        public string ServiceType { get; set; }

        public string ImplementType { get; set; }

        public LifetimeScope LifetimeScope { get; set; }

        #endregion 实例属性

        #region 实例方法

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("接口类型：{0}", ServiceType);
            sb.AppendLine();
            sb.AppendFormat("实现类型：{0}", ImplementType);
            sb.AppendLine();
            sb.AppendFormat("生命周期：{0}", LifetimeScope);
            return sb.ToString();
        }

        #endregion 实例方法
    }

    #endregion 组件类
}