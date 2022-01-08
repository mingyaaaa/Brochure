using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    /// <summary>
    /// The query params.
    /// </summary>
    public class QueryParams<T>
    {
        /// <summary>
        /// Gets or sets the where.
        /// </summary>
        public Func<T, bool> Where { get; set; }

        /// <summary>
        /// Gets or sets the filter field.
        /// </summary>
        public string[] FilterField { get; set; }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets the skip.
        /// </summary>
        public int Skip { get; set; }
    }
}