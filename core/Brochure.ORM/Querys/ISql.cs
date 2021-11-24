using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The sql.
    /// </summary>
    public interface ISql
    {
        /// <summary>
        /// Gets the s q l.
        /// </summary>
        string SQL { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        List<IDbDataParameter> Parameters { get; }
    }
}