using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    internal enum SelectObjType
    {
        New,
        ArrayField,
        Member,
    }

    public class SelectVisitor : ORMVisitor
    {
        /// <summary>
        /// Gets or sets the select type.
        /// </summary>
        internal Type SelectType { get; set; }

        private SelectObjType _selectObjType = SelectObjType.Member;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectVisitor"/> class.
        /// </summary>
        /// <param name="dbPrivoder">The db privoder.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="funcVisits">The func visits.</param>
        public SelectVisitor(IDbProvider dbPrivoder, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbPrivoder, dbOption, funcVisits)
        {
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            sql = _dbPrivoder.FormatFieldName(node.Value?.ToString());
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            _selectObjType = SelectObjType.ArrayField;
            var list = new List<string>();
            foreach (var item in node.Expressions)
            {
                var obj = base.GetSql(item);
                if (obj is not string)
                {
                    throw new Exception("Select 表达式参数类型不正确，需要传入列名");
                }
                list.Add(obj.ToString());
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
            _selectObjType = SelectObjType.New;
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
                var alis = member.Member.Name;
                list.Add($"{field} as {alis}");
            }
            sql = $"{string.Join(",", list)}";
            return node;
        }

        /// <summary>
        /// Visits the binary.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var left = base.GetSql(node.Left);
            var exType = node.NodeType;
            var right = base.GetSql(node.Right);
            sql = _dbPrivoder.GetOperateSymbol(left, exType, right);
            return node;
        }

        /// <summary>
        /// Visits the member.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (_selectObjType == SelectObjType.Member)
                SelectType = node.Member.DeclaringType;
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
            base.GetSql(expression);
            sql = $"select {sql} from";
            return sql;
        }

        /// <summary>
        /// Gets the parent express value.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A string.</returns>
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