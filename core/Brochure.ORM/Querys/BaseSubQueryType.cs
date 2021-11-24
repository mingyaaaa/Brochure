using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// 子查询类型
    /// </summary>
    public abstract class BaseSubQueryType
    {
        /// <summary>
        /// Gets the sub query type.
        /// </summary>
        /// <returns>An object.</returns>
        internal abstract object GetSubQueryType();
    }

    /// <summary>
    /// 子查询为表名
    /// </summary>
    public class TableNameSubQueryType : BaseSubQueryType
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNameSubQueryType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TableNameSubQueryType(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Gets the sub query type.
        /// </summary>
        /// <returns>An object.</returns>
        internal override object GetSubQueryType()
        {
            return _type;
        }
    }

    public class QuerySubQueryType : BaseSubQueryType
    {
        private readonly IQuery _query;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySubQueryType"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public QuerySubQueryType(IQuery query)
        {
            _query = query;
        }

        /// <summary>
        /// Gets the sub query type.
        /// </summary>
        /// <returns>An object.</returns>
        internal override object GetSubQueryType()
        {
            return _query;
        }
    }
}