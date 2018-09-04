using Brochure.Core;
using Brochure.Core.Server;

namespace Brochure.Server.MySql
{
    public class MySqlDbParams : IDbParams
    {
        public MySqlDbParams()
        {
            Params = new Record();
        }
        public string ParamSymbol => "@";
        public string Sql { get; set; }
        public IRecord Params { get; set; }
    }
}
