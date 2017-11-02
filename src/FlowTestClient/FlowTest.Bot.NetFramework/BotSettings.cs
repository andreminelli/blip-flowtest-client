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
        [DataMember(Name = "textdocs")]
        public List<TestDoc> TextDocs { get; set; }
    }

    [DataContract]
    public class TestDoc
    {
        [DataMember(Name = "command")]
        public string Command { get; set; }

        [DataMember(Name = "doc")]

        public string PlainDocument { get; set; }
    }
}
