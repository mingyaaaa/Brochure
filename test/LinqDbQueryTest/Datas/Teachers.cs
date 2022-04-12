using Brochure.ORM;
using System;

namespace Brochure.ORMTest
{
    /// <summary>
    /// The teachers.
    /// </summary>
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

    /// <summary>
    /// The f teachers.
    /// </summary>
    public class FTeachers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FTeachers"/> class.
        /// </summary>
        public FTeachers()
        {
            Teachers = new Teachers();
        }

        /// <summary>
        /// Gets or sets the teachers.
        /// </summary>
        public Teachers Teachers { get; set; }
    }

    /// <summary>
    /// The teachers model.
    /// </summary>
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