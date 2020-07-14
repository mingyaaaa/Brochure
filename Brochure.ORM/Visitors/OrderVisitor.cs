using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    public class OrderVisitor : ORMVisitor
    {
        private readonly bool isAes;

        public OrderVisitor (IDbProvider dbProvider, bool isAes = true) : base (dbProvider)
        {
            this.isAes = isAes;
        }

        protected override Expression VisitMemberInit (MemberInitExpression node)
        {
            var list = new List<string> ();
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                if (!(node.Bindings[i] is MemberAssignment member))
                    continue;
                var field = GetSql (member.Expression);
                list.Add ($"{field}");
            }
            sql = $"order by {string.Join(",", list)}";
            if (!isAes)
                sql = $"{sql} desc";
            return node;
        }

        protected override Expression VisitNew (NewExpression node)
        {
            var list = new List<string> ();
            var parms = node.Arguments;
            for (int i = 0; i < parms.Count; i++)
            {
                list.Add ($"{GetSql(parms[i])}");
            }
            sql = $"order by {string.Join(",", list)} ";
            if (!isAes)
                sql = $"{sql} desc";
            return node;
        }

        public override object GetSql (Expression expression = null)
        {
            if (expression != null)
            {
                base.Visit (expression);
            }
            else
            {
                var str = sql?.ToString () ?? string.Empty;
                if (!string.IsNullOrWhiteSpace (str) && !str.Contains ("order by"))
                {
                    sql = $"order by {str}";
                    if (!isAes)
                        sql = $"{sql} desc";
                }
            }
            return sql;
        }
    }
}