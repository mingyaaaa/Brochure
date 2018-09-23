namespace Brochure.Core.Interfaces
{
    public interface IContext
    {
        IAuthManager PluginAuth { get; }
    }
}
