using Brochure.ORM;

namespace Brochure.ORMTest
{
    public class Teachers : EntityBase, IEntityKey<string>
    {
        public string School { get; set; }

        public string Job { get; set; }
        public string Id { get; set; }
    }

    public class TeachersModel
    {
        public string School { get; set; }

        public string Job { get; set; }
    }
}