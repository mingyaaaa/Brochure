using System;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IStartupConfigureServices
    {
        void ConfigureService(IServiceCollection services);
    }
}
