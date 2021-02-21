using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public class ApplicationOption
    {
        public ApplicationOption (IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

    }
}