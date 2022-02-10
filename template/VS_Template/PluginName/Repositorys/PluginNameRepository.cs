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
    internal class $safeprojectname$Repository : RepositoryBase<$safeprojectname$Entrity, string>, I$safeprojectname$Repository
    {
        public $safeprojectname$Repository(DbData dbData, DbContext dbContext) : base(dbData, dbContext)
        {
        }
    }
}