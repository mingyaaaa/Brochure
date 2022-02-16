using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Abstract.Utils
{
    /// <summary>
    /// The file.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Exists the.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A bool.</returns>
        bool Exists(string filePath);

        /// <summary>
        /// Writes the all text async.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default);

        /// <summary>
        /// Writes the all text async.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the all text async.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the all text async.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
    }
}