using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Brochure.Core;
using test.Entity;
using Xunit;
using Xunit.Abstractions;

namespace test
{
    public class GrammarTest : BaseTest
    {
        public GrammarTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {

        }

        [Fact]
        public void Test1()
        {
            Expression<Func<UserEntity, bool>> expression = u => u.Age == 1;
            Expression<Func<UserEntity, object>> expression1 = u => u.Age;
            Expression<Func<UserEntity, int>> expressionaction = y => y.Age + 1;
            ParameterExpression number = Expression.Parameter(typeof(int), "number");

            BlockExpression myBlock = Expression.Block(
                new[] { number },
                Expression.Assign(number, Expression.Constant(2)),
                Expression.AddAssign(number, Expression.Constant(6)),
                Expression.DivideAssign(number, Expression.Constant(2)));
            Expression<Func<int>> myAction = Expression.Lambda<Func<int>>(myBlock);
            Console.WriteLine(myAction.Compile()());
        }

        private Expression Select(Expression<Action> expression)
        {
            return expression;
        }
    }
}
