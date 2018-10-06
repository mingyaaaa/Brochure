using Brochure.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Brochure.Core.Server.Implements
{
    public class Auths : IAuth
    {
        public Auths()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var jsonRecord = JsonUtil.ReadArrayJson(Path.Combine(basePath, "auth.json"));
            AuthModels = jsonRecord.OfType(t => t.As<AuthModel>());
        }
        public List<AuthModel> AuthModels { get; }
    }
}
