using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Brochure.ORMTest
{
    /// <summary>
    /// The test visitor.
    /// </summary>
    [TestClass]
    public class TestVisitor : BaseTest
    {
        private ORMVisitor visitor;
        private DbOption option;
        private IDbProvider provider;
        private Mock<TransactionManager> transactionManager;
        private ISqlBuilder _queryBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestVisitor"/> class.
        /// </summary>
        public TestVisitor()
        {
            provider = base.Provider.GetService<IDbProvider>();
            transactionManager = new Mock<TransactionManager>();
            option = base.Provider.GetService<DbOption>();
            _queryBuilder = Provider.GetService<ISqlBuilder>();
        }

        /// <summary>
        /// Tests the where visitor.
        /// </summary>
        [TestMethod]
        public void TestWhereVisitor()
        {
            option.IsUseParamers = false;
            visitor = new WhereVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, bool>> ex = t => t.Id == "1";
            var a = visitor.Visit(ex);
            var sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);

            Assert.AreEqual("`Peoples`.`Id` = '1'", sql.ToString());

            ex = t => t.Age == 1;
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Age` = 1", sql.ToString());

            int[] array = new int[] { 1, 10, 2 };

            ex = t => array.Contains(t.Age);
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Age` in (1,10,2)", sql.ToString());

            const string name = "aaa";
            ex = t => t.Name.Contains(name);
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Name` like '%aaa%'", sql.ToString());

            ex = t => t.Name.StartsWith(name);
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Name` like '%aaa'", sql.ToString());

            ex = t => t.Name.EndsWith(name);
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Name` like 'aaa%'", sql.ToString());

            ex = t => t.Name == null;
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Name` is null", sql.ToString());

            ex = t => t.Name != null;

            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Name` is not null", sql.ToString());
            ex = t => t.Age != 1;
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Age` != 1", sql.ToString());

            ex = t => t.Age == 1 && t.Name == "1";
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("`Peoples`.`Age` = 1 and `Peoples`.`Name` = '1'", sql.ToString());
        }

        /// <summary>
        /// Tests the select visitor.
        /// </summary>
        [TestMethod]
        public void TestSelectVisitor()
        {
            visitor = new SelectVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, object>> ex = t => new { NewName = t.Name, NewAge = t.Age };
            var a = visitor.Visit(ex);
            var sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null);
            Assert.AreEqual("select `Peoples`.`Name` as NewName,`Peoples`.`Age` as NewAge from", sql.ToString());
            ex = t => t.Age;
            visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null);
            Assert.AreEqual("select `Peoples`.`Age` from", sql.ToString());

            Expression<Func<Peoples, Students, object>> ex2 = (p, s) => new { NewName = p.Name, NewAge = p.Age, s.ClassId };
            a = visitor.Visit(ex2);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null);
            Assert.AreEqual("select `Peoples`.`Name` as NewName,`Peoples`.`Age` as NewAge,`Students`.`ClassId` as ClassId from", sql.ToString());
        }

        /// <summary>
        /// Tests the join visitor.
        /// </summary>
        [TestMethod]
        public void TestJoinVisitor()
        {
            visitor = new JoinVisitor(provider, option, new List<IFuncVisit>());
            ((JoinVisitor)visitor).SetTableName(typeof(Students));
            Expression<Func<Peoples, Students, bool>> ex = (p, s) => s.PeopleId == p.Id;
            visitor.Visit(ex);
            var sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, null);
            Assert.AreEqual("join `Students` on `Students`.`PeopleId` = `Peoples`.`Id`", sql.ToString());
        }

        /// <summary>
        /// Tests the group visitor.
        /// </summary>
        [TestMethod]
        public void TestGroupVisitor()
        {
            var visitor = new GroupVisitor(provider, option, new List<IFuncVisit>());
            Expression<Func<Peoples, object>> ex = t => new { t.Age, t.BirthDay };
            var a = visitor.Visit(ex);
            var sql = new StringBuilder(visitor.GetSql().ToString().Trim());
            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, visitor.GroupDic);
            Assert.AreEqual("group by `Peoples`.`Age`,`Peoples`.`BirthDay`", sql.ToString());

            ex = t => t.Age;
            a = visitor.Visit(ex);
            sql = new StringBuilder(visitor.GetSql().ToString().Trim());

            _queryBuilder.RenameTableType(sql, visitor.GetTableDic(), null, visitor.GroupDic);
            Assert.AreEqual("group by `Peoples`.`Age`", sql.ToString());
        }

        /// <summary>
        /// Tests the paramers.
        /// </summary>
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
            // Assert.AreEqual("`Peoples`.`Id` = @p0", sql);
        }
    }
}