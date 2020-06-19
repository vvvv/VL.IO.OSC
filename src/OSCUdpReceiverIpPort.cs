using System;
using VL.Lib.Collections;
using VL.Core;

namespace VL.IO.OSC
{
    public class OSCUdpReceiverIpPortDefinition : ManualDynamicEnumDefinitionBase<OSCUdpReceiverIpPortDefinition>
    {
        //add this to get a node that can access the Instance from everywhere
        public static OSCUdpReceiverIpPortDefinition OSCUdpReceiverIpPortDefinitionInstance => ManualDynamicEnumDefinitionBase<OSCUdpReceiverIpPortDefinition>.Instance;
    }

    [Serializable]
    public class OSCUdpReceiverIpPort : DynamicEnumBase<OSCUdpReceiverIpPort, OSCUdpReceiverIpPortDefinition>
    {
        public OSCUdpReceiverIpPort(string value) : base(value)
        {
        }

        //this method needs to be imported in VL to set the default
        public static OSCUdpReceiverIpPort CreateDefault()
        {
            //use method of base class if nothing special required
            return new OSCUdpReceiverIpPort("Server 1");
        }
    }
}
