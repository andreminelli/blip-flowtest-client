using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FlowTest.Bot.NetFramework
{
    [DataContract]
    public class BotSettings
    {
        [DataMember(Name = "testdocs")]
        public List<TestDoc> TestDoc { get; set; }
    }

    [DataContract]
    public class TestDoc
    {
        [DataMember(Name = "command")]
        public string Command { get; set; }

        [DataMember(Name = "doc")]

        public string RawDocument { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

    }
}
