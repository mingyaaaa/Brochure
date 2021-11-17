using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    public interface IDbBuilder
    {
        IServiceCollection Service { get; }
    }

    public class DbBuilder : IDbBuilder
    {
        private readonly IServiceCollection _services;

        public DbBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceCollection Service => _services;
    }
}