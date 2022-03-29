using Brochure.ORM;
using Brochure.ORM.Database;
using PluginTemplate.Entrities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTemplate.Repositorys
{
    internal class LoginRepository : RepositoryBase<LoginEntrity, string>, ILoginRepository
    {
        public LoginRepository(DbData dbData, DbContext dbContext) : base(dbData, dbContext)
        {
        }
    }
}