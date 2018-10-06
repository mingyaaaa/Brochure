using Brochure.Core.Models;
using System.Collections.Generic;

namespace Brochure.Core
{
    public interface IAuth
    {
        List<AuthModel> AuthModels { get; }
    }
}
