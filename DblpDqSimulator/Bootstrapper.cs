using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;

namespace DblpDqSimulator
{
    public static class Bootstrapper
    {
        public static IContainer Get()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DebugLogger>().As<ILogger>().SingleInstance();
            builder.Register(c => new Progress(c.Resolve<ILogger>(), 5)).As<IProgress>().SingleInstance();
            builder.Register(c => new Dal(c.Resolve<ILogger>(), c.Resolve<IProgress>()));
            return builder.Build();
        }
    }
}
