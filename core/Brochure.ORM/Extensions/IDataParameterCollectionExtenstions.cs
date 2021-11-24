using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
namespace Brochure.ORM.Extensions
{
    public static class IDataParameterCollectionExtenstions
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="parameters">The parameters.</param>
        public static void AddRange (this IDataParameterCollection collection, IEnumerable<IDbDataParameter> parameters)
        {
            parameters.ToList ().ForEach (t => collection.Add (t));
        }
    }
}