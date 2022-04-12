using Brochure.Abstract.Utils;
using System.IO;

namespace Brochure.Utils.SystemUtils
{
    /// <summary>
    /// The sys directory.
    /// </summary>
    public class SysDirectory : ISysDirectory
    {
        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="searchParttern">The search parttern.</param>
        /// <param name="searchOption">The search option.</param>
        /// <returns>An array of string.</returns>
        public string[] GetFiles(string filePath, string searchParttern, SearchOption searchOption)
        {
            if (!Directory.Exists(filePath))
                return new string[0];
            return Directory.GetFiles(filePath, searchParttern, searchOption);
        }
    }
}