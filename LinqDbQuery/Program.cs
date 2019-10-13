using System;
using System.Linq.Expressions;
using LinqDbQuery.Visitors;

namespace LinqDbQuery
{
    public static class Program
    {
        private static void Main (string[] args)
        {
            if (args == null)
                throw new ArgumentNullException (nameof (args));
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}