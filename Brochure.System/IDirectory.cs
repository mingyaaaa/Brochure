using System.IO;
namespace Brochure.System
{
    public interface ISysDirectory
    {
        string[] GetFiles (string filePath, string searchParttern, SearchOption searchOption);
    }

    public class SysDirectory : ISysDirectory
    {
        public string[] GetFiles (string filePath, string searchParttern, SearchOption searchOption)
        {
            return Directory.GetFiles (filePath, searchParttern, searchOption);
        }
    }
}