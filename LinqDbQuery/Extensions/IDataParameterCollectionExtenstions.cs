using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
namespace LinqDbQuery.Extensions
{
    public static class IDataParameterCollectionExtenstions
    {
        public static void AddRange (this IDataParameterCollection collection, IEnumerable<IDbDataParameter> parameters)
        {
            parameters.ToList ().ForEach (t => collection.Add (t));
        }
    }
}