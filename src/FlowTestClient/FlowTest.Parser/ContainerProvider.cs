﻿using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTest.Parser
{
    public static class ContainerProvider
    {

        public static Container DefaultRegister(Container container)
        {
            container.RegisterSingleton<IScriptReader, ScriptReader>();
            container.RegisterSingleton<IMessageProvider, MessageProvider>();

            return container;
        }
    }
}
