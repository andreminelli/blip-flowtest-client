using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTestClient.Models
{
    public abstract class TestMessage
    {
        public string RawTextContent { get; set; }

    }

    public class UnexpectedTestMessage : TestMessage
    {
    }

}
