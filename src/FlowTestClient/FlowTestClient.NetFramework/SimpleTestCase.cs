using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTestClient.NetFramework.Tests
{
    [TestClass]
    public class SimpleTestCase
    {

        [TestMethod]
        [DataRow("Examples/ex1.txt")]
        public void SimpleTest(string filename)
        {
            using (var container = new Container())
            {
                ContainerProvider.DefaultRegister(container);
                container.Verify();

                var scriptReader = container.GetInstance<IScriptReader>();


                var r = scriptReader.ExtractFlowTestAsync(filename).Result;


                Assert.IsTrue(r != null && r.TestMessagesQueue.Count > 0); 
            }
        }

    }
}
