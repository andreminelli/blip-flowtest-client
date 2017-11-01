using FlowTestClient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTestClient.NetFramework.Tests
{
    [TestClass]
    public class MessageProviderTests
    {
        [TestMethod]
        [DataRow("> oi", "oi", DisplayName = "Case1")]
        [DataRow(@"> oi\noi", "oi\noi", DisplayName = "Case2")]
        public void Build_Message_ToBot_With_Text(string input, string expectedText)
        {
            expectedText = expectedText.Replace("\n", Environment.NewLine);

            var messageProvider = new MessageProvider();

            var message = messageProvider.BuildMessage(input);

            Assert.IsInstanceOfType(message, typeof(ToBotMessage));
            Assert.AreEqual(expectedText, message.RawTextContent);

        }

    }
}
