using System;
using System.Linq.Expressions;
using LinqDbQuery.Visitors;

namespace LinqDbQuery
{
    public class Program
    {
        private static void Main (string[] args) { }
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}