using LinqDbQuery;
using System.Collections.Generic;

namespace LinqDbQuery.A
{
    public abstract class Sql
    {
        private ISqlParse _parse;
        public Sql(ISqlParse parse)
        {
            _parse = parse;
        }
        protected virtual string ToSql()
        {
            return _parse.Parse(this);
        }
        public override string ToString()
        {
            return ToSql();
        }
    }
    public interface ISqlParse
    {
        string Parse(Sql sql);
    }
    public class SelectSql : Sql
    {
        public bool IsSelectAll;
        public SelectSql(ISqlParse parse) : base(parse)
        {
            Colomns = new List<string>();
        }
        public List<string> Colomns { get; set; }

    }

    public class TableSql : Sql
    {
        public TableSql(ISqlParse parse) : base(parse)
        {
        }
    }
    public class JoinSql : Sql
    {
        public JoinSql(ISqlParse parse) : base(parse)
        {
        }
    }

    public class WhereSql : Sql
    {
        public WhereSql(ISqlParse parse) : base(parse)
        {
        }
    }
    public class GroupSql : Sql
    {
        public GroupSql(ISqlParse parse) : base(parse)
        {
        }
    }
    public class OrderSql : Sql
    {
        public OrderSql(ISqlParse parse) : base(parse)
        {
        }
    }

    public class UnionSql
    {
    }
}
