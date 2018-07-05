using System.Collections.Generic;
using Brochure.Core.Interfaces;
using Brochure.Core.Server.Interfaces;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlDbParams : IDbParams
    {
        public string ParamSymbol => "";
        public string Sql { get; set; }
        public IBDocument Params { get; set; }
    }
}