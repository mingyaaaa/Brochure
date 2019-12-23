using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Extensions;
namespace Brochure.Core
{
    public class BaseBootstrap : IBootstrap
    {
        public async Task Exit (IPlugins[] plugins)
        {
            foreach (var item in plugins)
            {
                await item.ExitAsync ();
            }
        }

        public Task Start ()
        {
            //注入转换器
            ObjectConverCollection.RegistObjectConver<IRecord> (t => new Record (t.AsDictionary ()));
            return Task.CompletedTask;
        }
    }
}