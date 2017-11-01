using FlowTestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTestClient
{
    public interface IMessageProvider
    {
        TestMessage BuildMessage(string input);
    }
}
