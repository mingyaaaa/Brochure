using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Brochure.LinqDbQuery.MySql;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.ORMTest
{
    [TestClass]
    public class TestVisitor : BaseTest
    {
        private ORMVisitor visitor;
        private DbOption option;
        IDbProvider provider;
        private Mock<TransactionManager> transactionManager;
        public TestVisitor()
        {
            provider = base.Provider.GetService<IDbProvider>();
            transactionManager = new Mock<TransactionManager>();
            option = base.Provider.GetService<DbOption>();
        }

        [TestMethod]
        public void TestWhereVisitor()
        {
            option.IsUseParamers = false;
            visitor = new WhereVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, bool>> ex = t => t.Id == "1";
            var a = visitor.Visit(ex);
            var sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Id] = '1'", sql);

            ex = t => t.Age == 1;
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Age] = 1", sql);

            int[] array = new int[] { 1, 10, 2 };

            ex = t => array.Contains(t.Age);
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Age] in (1,10,2)", sql);

            const string name = "aaa";
            ex = t => t.Name.Contains(name);
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Name] like '%aaa%'", sql);

            ex = t => t.Name.StartsWith(name);
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Name] like '%aaa'", sql);

            ex = t => t.Name.EndsWith(name);
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Name] like 'aaa%'", sql);

            ex = t => t.Name == null;
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Name] is null", sql);

            ex = t => t.Name != null;

            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Name] is not null", sql);
            ex = t => t.Age != 1;
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Age] != 1", sql);

            ex = t => t.Age == 1 && t.Name == "1";
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("where [Peoples].[Age] = 1 and [Peoples].[Name] = '1'", sql);
        }

        [TestMethod]
        public void TestSelectVisitor()
        {
            visitor = new SelectVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, object>> ex = t => new { NewName = t.Name, NewAge = t.Age };
            var a = visitor.Visit(ex);
            var sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("select [Peoples].[Name] as NewName,[Peoples].[Age] as NewAge from", sql);
            ex = t => t.Age;
            visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("select [Peoples].[Age] from", sql);

            Expression<Func<Peoples, Students, object>> ex2 = (p, s) => new { NewName = p.Name, NewAge = p.Age, s.ClassId };
            a = visitor.Visit(ex2);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("select [Peoples].[Name] as NewName,[Peoples].[Age] as NewAge,[Students].[ClassId] as ClassId from", sql);
        }

        [TestMethod]
        public void TestJoinVisitor()
        {
            visitor = new JoinVisitor(provider, option, new List<IFuncVisit>());
            ((JoinVisitor)visitor).SetTableName(typeof(Students));
            Expression<Func<Peoples, Students, bool>> ex = (p, s) => s.PeopleId == p.Id;
            visitor.Visit(ex);
            var sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("join [Students] on [Students].[PeopleId] = [Peoples].[Id]", sql);
        }

        [TestMethod]
        public void TestGroupVisitor()
        {
            visitor = new GroupVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, object>> ex = t => new { t.Age, t.BirthDay };
            var a = visitor.Visit(ex);
            var sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("group by [Peoples].[Age],[Peoples].[BirthDay]", sql);

            ex = t => t.Age;
            a = visitor.Visit(ex);
            sql = visitor.GetSql().ToString().Trim();
            Assert.AreEqual("group by [Peoples].[Age]", sql);
        }

        [TestMethod]
        public void TestParamers()
        {
            visitor = new WhereVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, bool>> ex = t => t.Id == "1";
            var a = visitor.Visit(ex);
            var sql = visitor.GetSql().ToString().Trim();
            var parmas = visitor.GetParameters();
            Assert.AreEqual(1, parmas.Count());
            Assert.AreEqual("1", parmas.First().Value);
            Assert.AreEqual("where [Peoples].[Id] = @p0", sql);
        }
    }
}