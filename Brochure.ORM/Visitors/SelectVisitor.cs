using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.ORM.Visitors
{
    public class SelectVisitor : ORMVisitor
    {
        public SelectVisitor (IDbProvider dbPrivoder) : base (dbPrivoder) { }

        protected override Expression VisitNew (NewExpression node)
        {
            var list = new List<string> ();
            var parms = node.Arguments;
            var members = node.Members;
            for (int i = 0; i < parms.Count; i++)
            {
                var member = members[i];
                list.Add ($"{GetSql(parms[i])} as {member.Name}");
            }
            sql = $"select {string.Join(",", list)} from ";
            return node;
        }

        protected override Expression VisitMemberInit (MemberInitExpression node)
        {
            var list = new List<string> ();
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                if (!(node.Bindings[i] is MemberAssignment member))
                    continue;
                var field = GetSql (member.Expression);
                var alis = member.Member.Name;
                list.Add ($"{field} as {alis}");
            }
            sql = $"select {string.Join(",", list)} from ";
            return node;
        }

        protected override Expression VisitBinary (BinaryExpression node)
        {
            var left = GetSql (node.Left);
            var exType = node.NodeType;
            var right = GetSql (node.Right);
            sql = _dbPrivoder.GetOperateSymbol (left, exType, right);
            return node;
        }

        public override object GetSql (Expression expression = null)
        {
            if (expression != null)
            {
                return base.GetSql (expression);
            }
            else
            {
                var sqlStr = sql?.ToString () ?? string.Empty;
                if (!string.IsNullOrWhiteSpace (sqlStr) && !sqlStr.Contains ("select"))
                    return $"select {sql} from ";
                else
                    return sqlStr;
            }
        }
    }
}