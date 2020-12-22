﻿using System;
using System.Threading.Tasks;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class ServiceCollectionExtendsTest
    {
        IServiceCollection services;
        public ServiceCollectionExtendsTest()
        {
            services = new ServiceCollection();
            services.AddLogging();
        }

        [TestMethod]
        public void TestAddBrochureCore()
        {
            services.AddBrochureCore();
        }
    }
}