using System.Collections.Generic;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Brochure.Core.Server.Interfaces;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlDbParams : IDbParams
    {
        public MySqlDbParams ()
        {
            Params = new Record ();
        }
        public string ParamSymbol => "";
        public string Sql { get; set; }
        public IRecord Params { get; set; }
    }
}