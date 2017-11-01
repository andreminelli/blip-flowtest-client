using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTest.Contracts.Models
{
    public class FlowTestCase
    {

        public Queue<TestMessage> TestMessagesQueue { get; private set; }

        public FlowTestCase()
        {
            TestMessagesQueue = new Queue<TestMessage>();
        }

        public void AddTestMessage(TestMessage testMessage)
        {
            TestMessagesQueue.Enqueue(testMessage);
        }
    }
}
