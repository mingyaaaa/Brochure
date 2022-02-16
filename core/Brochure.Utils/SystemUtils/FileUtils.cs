using Brochure.Abstract.Utils;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Utils.SystemUtils
{
    /// <summary>
    /// The file utils.
    /// </summary>
    public class FileUtils : IFile
    {
        /// <summary>
        /// Exists the.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A bool.</returns>
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.ReadAllTextAsync(path, encoding, cancellationToken);
        }

        public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        {
            return File.ReadAllTextAsync(path, cancellationToken);
        }

        public Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
        }

        /// <summary>
        /// Writes the all text async.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            return File.WriteAllTextAsync(path, contents, cancellationToken);
        }
    }
}