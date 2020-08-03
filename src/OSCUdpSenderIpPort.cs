using System;
using VL.Lib.Collections;
using VL.Core;

namespace VL.IO.OSC
{
    public class OSCUdpSenderIpPortDefinition : ManualDynamicEnumDefinitionBase<OSCUdpSenderIpPortDefinition>
    {
        //add this to get a node that can access the Instance from everywhere
        public static OSCUdpSenderIpPortDefinition OSCUdpSenderIpPortDefinitionInstance => ManualDynamicEnumDefinitionBase<OSCUdpSenderIpPortDefinition>.Instance;
    }

    [Serializable]
    public class OSCUdpSenderIpPort : DynamicEnumBase<OSCUdpSenderIpPort, OSCUdpSenderIpPortDefinition>
    {
        public OSCUdpSenderIpPort(string value) : base(value)
        {
        }

        //this method needs to be imported in VL to set the default
        public static OSCUdpSenderIpPort CreateDefault()
        {
            //use method of base class if nothing special required
            return new OSCUdpSenderIpPort("Client 1");
        }
    }
}
