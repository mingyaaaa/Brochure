using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    public class SelectVisitor : ORMVisitor
    {
        internal Type SelectType { get; set; }
        private bool isSetNew = false;

        public SelectVisitor(IDbProvider dbPrivoder, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbPrivoder, dbOption, funcVisits)
        {
        }

        protected override Expression VisitNew(NewExpression node)
        {
            isSetNew = true;
            var list = new List<string>();
            var parms = node.Arguments;
            var members = node.Members;
            for (int i = 0; i < parms.Count; i++)
            {
                var member = members[i];
                list.Add($"{base.GetSql(parms[i])} as {member.Name}");
            }
            sql = $"{string.Join(",", list)}";
            SelectType = node.Type;
            return node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var list = new List<string>();
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                if (!(node.Bindings[i] is MemberAssignment member))
                    continue;
                var field = base.GetSql(member.Expression);
                var alis = member.Member.Name;
                list.Add($"{field} as {alis}");
            }
            sql = $"{string.Join(",", list)}";
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var left = base.GetSql(node.Left);
            var exType = node.NodeType;
            var right = base.GetSql(node.Right);
            sql = _dbPrivoder.GetOperateSymbol(left, exType, right);
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!isSetNew)
                SelectType = node.Member.DeclaringType;
            sql = GetParentExpressValue(node);
            return node;
        }

        public override object GetSql(Expression expression = null)
        {
            base.GetSql(expression);
            sql = $"select {sql} from";
            return sql;
        }

        private string GetParentExpressValue(MemberExpression expression)
        {
            string memberName = _dbPrivoder.FormatFieldName(expression.Member.Name);
            if (expression.Expression is MemberExpression memberExpression)
            {
                var str = GetParentExpressValue(memberExpression);
                return $"{str}.{memberName}";
            }
            else if (expression.Expression is ParameterExpression parameterExpression)
            {
                //处理Group类型的问题
                var tableKey = parameterExpression.Type.GetHashCode();
                if (parameterExpression.Type.Name == typeof(IGrouping<,>).Name)
                {
                    return $"{parameterExpression.Type.Name}.{memberName}";
                }
                else
                {
                    TableTypeDic.TryAdd(tableKey, parameterExpression.Type);
                }
                return $"{tableKey}.{memberName}";
            }

            return "";
        }
    }
}