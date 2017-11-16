using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FlowTest.Contracts.Models
{
    public class BlipTestHostChannelSettings
    {
        public Assembly AssemblyReference { get; set; }
        public TimeSpan MessageTimeout { get; set; }
        public TimeSpan NotificationTimeout { get; set; }
    }
}
