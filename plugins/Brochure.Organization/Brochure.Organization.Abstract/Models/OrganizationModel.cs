namespace Brochure.Organization.Abstract.Models
{
    /// <summary>
    /// The organization model.
    /// </summary>
    public class OrganizationModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentId { get; set; }
    }
}