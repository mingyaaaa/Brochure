using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.User.Test
{
    public class BaseTest
    {
        protected IServiceProvider Service;
        protected IFixture Fixture;
        protected string Host;

        public BaseTest()
        {
            //var service = new ServiceCollection();
            //ConfigureService(service);
            //Service = service.BuildServiceProvider();
            //Fixture = new Fixture();
            //var configurationBuild = new ConfigurationBuilder();
            //var configurationRoot = configurationBuild.AddJsonFile("setting.json").Build();
            //Host = configurationRoot.GetValue<string>("Host", "http://localhost:5000");
        }

        public virtual void ConfigureService(IServiceCollection services)
        {
        }
    }
}