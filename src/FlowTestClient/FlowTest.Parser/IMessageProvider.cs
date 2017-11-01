using FlowTest.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTest.Parser
{
    public interface IMessageProvider
    {
        TestMessage BuildMessage(string input);
    }
}
