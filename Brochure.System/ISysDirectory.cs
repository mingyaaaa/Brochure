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
            if (!Directory.Exists (filePath))
                return new string[0];
            return Directory.GetFiles (filePath, searchParttern, searchOption);
        }
    }
}