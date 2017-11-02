using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTest.Contracts.Models
{
    public class FromBotMessage : TestMessage
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string ContentType { get; set; }


        public int OptionsNumber => Options == null ? 0 : Options.Count;
        public bool HasText => !string.IsNullOrWhiteSpace(Text);

    }
}
