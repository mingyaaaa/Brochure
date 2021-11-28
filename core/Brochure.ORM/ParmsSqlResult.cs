using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The parms sql result.
    /// </summary>
    public class ParmsSqlResult : ISqlResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParmsSqlResult"/> class.
        /// </summary>
        public ParmsSqlResult()
        {
            this.Parameters = new List<IDbDataParameter>();
        }

        /// <summary>
        /// Gets or sets the s q l.
        /// </summary>
        public string SQL { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public List<IDbDataParameter> Parameters { get; }
    }
}