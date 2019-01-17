using Surging.Core.CPlatform.Support;
using System.Collections.Generic;
using System.Net;

namespace Surging.Core.CPlatform.Configurations
{
    public partial class SurgingServerOptions : ServiceCommand
    {
        /// <summary>
        /// 微服务主机地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 如果使用Docker容器,则表示宿主机映射的IP
        /// </summary>
        public string MappingIP { get; set; }

        public string WanIp { get; set; }

        /// <summary>
        /// 如果使用Docker容器,则表示宿主机映射的端口号
        /// </summary>
        public int MappingPort { get; set; }

        /// <summary>
        /// 设置服务心跳间隔
        /// </summary>
        public double WatchInterval { get; set; } = 20d;

        public bool Libuv { get; set; } = false;

        public IPEndPoint IpEndpoint { get; set; }

        public List<ModulePackage> Packages { get; set; } = new List<ModulePackage>();

        /// <summary>
        /// 通信协议
        /// </summary>
        public CommunicationProtocol Protocol { get; set; }

        /// <summary>
        /// 设置服务模块的路径
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// 指定的微服务主机端口号
        /// </summary>
        public int Port { get; set; }

        public int SoBacklog { get; set; } = 8192;

        public bool EnableRouteWatch { get; set; }

        /// <summary>
        /// 通信协议端口
        /// </summary>
        public ProtocolPortOptions Ports { get; set; } = new ProtocolPortOptions();

        public string Token { get; set; } = "True";

        public string NotRelatedAssemblyFiles { get; set; }

        public string RelatedAssemblyFiles { get; set; } = "";

        public RuntimeEnvironment Environment { get; set; } = RuntimeEnvironment.Production;

        public bool ForceDisplayStackTrace { get; set; } = false;
    }
}