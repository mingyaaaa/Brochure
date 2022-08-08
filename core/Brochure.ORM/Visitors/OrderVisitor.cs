using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The order visitor.
    /// </summary>
    public class OrderVisitor : ORMVisitor
    {
        /// <summary>
        /// Gets or sets a value indicating whether is aes.
        /// </summary>
        public bool IsAes { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderVisitor"/> class.
        /// </summary>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="funcVisits">The func visits.</param>
        public OrderVisitor(IDbProvider dbProvider, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbProvider, dbOption, funcVisits) { }

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
        public override object GetSql(Expression? expression = null)
        {
            base.GetSql(expression);
            sql = $"order by {sql}";
            if (!IsAes)
                sql = $"{sql} desc";
            return sql;
        }
    }
}