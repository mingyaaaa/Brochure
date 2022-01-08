using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The group visitor.
    /// </summary>
    public class GroupVisitor : ORMVisitor
    {
        /// <summary>
        /// Gets the group dic.
        /// </summary>
        public Dictionary<string, string> GroupDic { get; }
        private bool isSetNew = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupVisitor"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="funcVisits">The func visits.</param>
        public GroupVisitor(IDbProvider provider, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(provider, dbOption, funcVisits)
        {
            GroupDic = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the parent express value.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A string.</returns>
        private string GetParentExpressValue(MemberExpression expression)
        {
            if (expression.Expression is MemberExpression memberExpression)
            {
                var str = GetParentExpressValue(memberExpression);
                return $"{str}.{expression.Member.Name}";
            }
            else if (expression.Expression is ParameterExpression parameterExpression)
            {
                var tableKey = parameterExpression.GetHashCode();
                TableTypeDic.TryAdd(tableKey, parameterExpression.Type);
                string memberName = _dbPrivoder.FormatFieldName(expression.Member.Name);
                var memberStr = isSetNew ? $".{memberName}" : string.Empty;
                GroupDic.TryAdd($"{typeof(IGrouping<,>).Name}.{_dbPrivoder.FormatFieldName("Key")}{memberStr}", $"{tableKey}.{memberName}");
                return $"{tableKey}.{memberName}";
            }

            return "";
        }

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
            isSetNew = true;
            for (int i = 0; i < parms.Count; i++)
            {
                list.Add($"{base.GetSql(parms[i])}");
            }
            sql = $"{string.Join(",", list)}";
            return node;
        }

        /// <summary>
        /// Visits the member.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            sql = GetParentExpressValue(node);
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