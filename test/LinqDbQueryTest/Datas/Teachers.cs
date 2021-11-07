using Brochure.ORM;
using System;

namespace Brochure.ORMTest
{
    public class Teachers : EntityBase, IEntityKey<string>
    {
        public string School { get; set; }

        public string Job { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public class FTeachers
    {
        public FTeachers()
        {
            Teachers = new Teachers();
        }

        public Teachers Teachers { get; set; }
    }

    public class TeachersModel
    {
        public string School { get; set; }

        public string Job { get; set; }
    }
}