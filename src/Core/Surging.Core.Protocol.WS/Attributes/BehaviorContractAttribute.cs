using System;

namespace Surging.Core.Protocol.WS.Attributes
{
    public class BehaviorContractAttribute : Attribute
    {
        public bool IgnoreExtensions { get; set; }
        public bool EmitOnPing { get; set; }
        public string Protocol { get; set; }
    }
}