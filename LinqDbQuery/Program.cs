using System;
using System.Linq.Expressions;
using LinqDbQuery.Visitors;

namespace LinqDbQuery
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Expression<Func<User, bool>> wheretree = t => t.Age == 1;
            var vis = new WhereVisitor(new MySqlDbProvider());
            vis.Visit(wheretree);

            Console.WriteLine(vis.GetSql());
            Console.ReadKey();
        }
    }
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}