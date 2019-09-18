using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
{
    public class GroupVisitor : ORMVisitor
    {
        public GroupVisitor (IDbProvider provider)
        {
            this._dbPrivoder = provider;
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
            sql = $"group by {string.Join(",", list)} from ";
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
            sql = $"group by {string.Join(",", list)} ";
            return node;
        }
    }
}