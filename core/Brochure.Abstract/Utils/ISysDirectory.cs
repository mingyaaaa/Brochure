using System.IO;
namespace Brochure.Abstract.Utils
{
    /// <summary>
    /// The sys directory.
    /// </summary>
    public interface ISysDirectory
    {
        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="searchParttern">The search parttern.</param>
        /// <param name="searchOption">The search option.</param>
        /// <returns>An array of string.</returns>
        string[] GetFiles(string filePath, string searchParttern, SearchOption searchOption);
    }
}