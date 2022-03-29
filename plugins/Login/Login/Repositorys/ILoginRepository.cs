using Brochure.ORM;
using PluginTemplate.Entrities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTemplate.Repositorys
{
    internal interface ILoginRepository : IRepository<LoginEntrity, string>
    {
    }
}