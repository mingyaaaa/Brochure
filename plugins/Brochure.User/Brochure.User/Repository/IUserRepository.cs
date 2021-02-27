using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Core.Server.Interfaces;
using Brochure.ORM;
using Brochure.User.Entrities;

namespace Brochure.User.Repository
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public interface IUserRepository : IRepository<UserEntrity>
    { }
}