namespace Brochure.ORMTest
{
    /// <summary>
    /// The students.
    /// </summary>
    public class Students
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the school.
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// Gets or sets the class id.
        /// </summary>
        public string ClassId { set; get; }

        /// <summary>
        /// Gets or sets the people id.
        /// </summary>
        public string PeopleId { get; set; }

        /// <summary>
        /// Gets or sets the class count.
        /// </summary>
        public int ClassCount { get; set; }

        /// <summary>
        /// Gets or sets the no.
        /// </summary>
        public decimal No { get; set; }
    }
}