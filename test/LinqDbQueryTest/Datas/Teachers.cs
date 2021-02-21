using Brochure.ORM;

namespace Brochure.ORMTest
{
    public class Teachers : EntityBase
    {
        public string School { get; set; }

        public string Job { get; set; }
    }

    public class TeachersModel
    {
        public string School { get; set; }

        public string Job { get; set; }
    }
}