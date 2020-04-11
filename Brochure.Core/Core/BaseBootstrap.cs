using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Extensions;
namespace Brochure.Core
{
    public class BaseBootstrap : IBootstrap
    {
        public Task Exit(IPlugins[] plugins)
        {
            return Task.CompletedTask;
        }

        public Task Start()
        {
            //注入转换器
            ObjectConverCollection.RegistObjectConver<IRecord>(t => new Record(t.AsDictionary()));
            return Task.CompletedTask;
        }
    }
}