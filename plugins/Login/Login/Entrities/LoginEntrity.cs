using Brochure.ORM;

namespace PluginTemplate.Entrities
{
    public class LoginEntrity : EntityBase, IEntityKey<string>
    {
        public string Id { get; set; }
    }
}