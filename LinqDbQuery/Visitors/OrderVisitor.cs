using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
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
                var member = node.Bindings[i] as MemberAssignment;
                if (member == null)
                    continue;
                var field = GetSql (member.Expression);
                var alis = member.Member.Name;
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
            var members = node.Members;
            for (int i = 0; i < parms.Count; i++)
            {
                var member = members[i];
                var alisName = member.Name;
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