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
        internal abstract object GetSubQueryType();
    }

    /// <summary>
    /// 子查询为表名
    /// </summary>
    public class TableNameSubQueryType : BaseSubQueryType
    {
        private readonly Type _type;

        public TableNameSubQueryType(Type type)
        {
            _type = type;
        }

        internal override object GetSubQueryType()
        {
            return _type;
        }
    }

    public class QuerySubQueryType : BaseSubQueryType
    {
        private readonly IQuery _query;

        public QuerySubQueryType(IQuery query)
        {
            _query = query;
        }

        internal override object GetSubQueryType()
        {
            return _query;
        }
    }
}