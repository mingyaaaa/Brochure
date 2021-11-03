using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    public class GroupVisitor : ORMVisitor
    {
        public GroupVisitor(IDbProvider provider, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(provider, dbOption, funcVisits) { }

        /// <summary>
        /// Visits the member init.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
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

        /// <summary>
        /// Visits the new.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
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

        /// <summary>
        /// Gets the sql.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An object.</returns>
        public override object GetSql(Expression expression = null)
        {
            base.Visit(expression);
            sql = $"group by {sql}";
            return sql;
        }
    }
}