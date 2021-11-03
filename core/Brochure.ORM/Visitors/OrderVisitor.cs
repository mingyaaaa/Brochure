using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    public class OrderVisitor : ORMVisitor
    {
        public bool IsAes { get; set; } = true;

        public OrderVisitor(IDbProvider dbProvider, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbProvider, dbOption, funcVisits) { }
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var list = new List<string>();
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                if (!(node.Bindings[i] is MemberAssignment member))
                    continue;
                var field = base.GetSql(member.Expression);
                list.Add($"{field}");
            }
            sql = $"{string.Join(",", list)}";
            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            var list = new List<string>();
            var parms = node.Arguments;
            for (int i = 0; i < parms.Count; i++)
            {
                list.Add($"{base.GetSql(parms[i])}");
            }
            sql = $"{string.Join(",", list)}";
            return node;
        }

        public override object GetSql(Expression expression = null)
        {
            base.GetSql(expression);
            sql = $"order by {sql}";
            if (!IsAes)
                sql = $"{sql} desc";
            return sql;
        }
    }
}