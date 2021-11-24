using Brochure.ORM;
using System;

namespace Brochure.ORMTest
{
    public class Teachers : EntityBase, IEntityKey<string>
    {
        /// <summary>
        /// Gets or sets the school.
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public class FTeachers
    {
        public FTeachers()
        {
            Teachers = new Teachers();
        }

        /// <summary>
        /// Gets or sets the teachers.
        /// </summary>
        public Teachers Teachers { get; set; }
    }

    public class TeachersModel
    {
        /// <summary>
        /// Gets or sets the school.
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        public string Job { get; set; }
    }
}