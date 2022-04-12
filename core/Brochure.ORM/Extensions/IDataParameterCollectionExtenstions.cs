using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Brochure.ORM.Extensions
{
    /// <summary>
    /// The i data parameter collection extenstions.
    /// </summary>
    public static class IDataParameterCollectionExtenstions
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="parameters">The parameters.</param>
        public static void AddRange(this IDataParameterCollection collection, IEnumerable<IDbDataParameter> parameters)
        {
            parameters.ToList().ForEach(t => collection.Add(t));
        }
    }
}