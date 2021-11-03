using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    public class ParmsSqlResult
    {
        public ParmsSqlResult()
        {
            this.Parameters = new List<IDbDataParameter>();
        }

        public string SQL { get; set; }

        public List<IDbDataParameter> Parameters;
    }
}