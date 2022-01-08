using System;
using System.Data;
using Brochure.ORM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LinqDbQueryTest.Querys
{
    /// <summary>
    /// The entriry conver test.
    /// </summary>
    [TestClass]
    public class EntriryConverTest
    {
        /// <summary>
        /// Tests the conver i data reader.
        /// </summary>
        [TestMethod]
        public void TestConverIDataReader()
        {
            var dataReaderMock = new Mock<IDataReader>();
            dataReaderMock.SetupSequence(t => t.Read()).Returns(true).Returns(true).Returns(false);
            dataReaderMock.SetupGet(t => t.FieldCount).Returns(3);
            dataReaderMock.SetupSequence(t => t.GetName(It.IsAny<int>())).Returns("Int1").Returns("String2").Returns("Id");
            dataReaderMock.SetupSequence(t => t["Int1"]).Returns(1).Returns(2);
            dataReaderMock.SetupSequence(t => t["String2"]).Returns("str1").Returns("str2");
            dataReaderMock.SetupSequence(t => t["Id"]).Returns(string.Empty).Returns(Guid.Parse("d275cd32-4eb7-4089-9ca8-5b96e5fbd85f").ToString());
            //var test = DbUtlis.ConverFromIDataReader<Test> (dataReaderMock.Object);
            //Assert.AreEqual (1, test.Int1);
            //Assert.AreEqual ("str1", test.String2);
            //Assert.AreEqual (string.Empty, test.Id);
        }
    }

    /// <summary>
    /// The test.
    /// </summary>
    public class Test : EntityBase
    {
        /// <summary>
        /// Gets or sets the int1.
        /// </summary>
        public int Int1 { get; set; }
        /// <summary>
        /// Gets or sets the string2.
        /// </summary>
        public string String2 { get; set; }
    }
}