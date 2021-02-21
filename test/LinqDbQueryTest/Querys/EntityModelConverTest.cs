using System;
using Brochure.Core;
using Brochure.ORMTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqDbQueryTest.Querys
{
    [TestClass]
    public class EntityModelConverTest
    {
        [TestMethod]
        public void EntityToModelConverTest ()
        {
            var obj = new ObjectFactory ();
            var guid = Guid.NewGuid ().ToString ();
            var teacherEntiry = new Teachers ()
            {
                Id = guid,
                Job = "job",
                School = "school",
                SequenceId = 1,
            };
            var model = obj.Create<Teachers, TeachersModel> (teacherEntiry);
            //   Assert.AreEqual (teacherEntiry.Id, model.Id);
            Assert.AreEqual (teacherEntiry.Job, model.Job);
            Assert.AreEqual (teacherEntiry.School, model.School);
            //  Assert.AreEqual (teacherEntiry.SequenceId, model.SequenceId);
        }
    }
}