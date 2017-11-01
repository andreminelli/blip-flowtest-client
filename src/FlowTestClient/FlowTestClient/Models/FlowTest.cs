using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTestClient.Models
{
    public class FlowTest
    {

        public string User { get; internal set; }
        public string Channel { get; internal set; }
        public string Identifier { get; internal set; }
        public string AccessKey { get; internal set; }

        public Queue<TestMessage> TestMessagesQueue { get; private set; }

        public FlowTest()
        {
            TestMessagesQueue = new Queue<TestMessage>();
        }

        public void AddTestMessage(TestMessage testMessage)
        {
            TestMessagesQueue.Enqueue(testMessage);
        }
    }
}
