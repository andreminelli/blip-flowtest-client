using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTest.Contracts.Models
{
    public abstract class TestMessage
    {
        public string RawTextContent { get; set; }

    }

    public class UnexpectedTestMessage : TestMessage
    {
    }

}
