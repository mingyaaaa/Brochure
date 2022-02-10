using Brochure.Abstract;
using PluginTemplate.Entrities;

namespace PluginTemplate.Dals
{
    public interface I$safeprojectname$Dal
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<$safeprojectname$Entrity>> Get$safeprojectname$(IEnumerable<string> ids);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="queryParams">The query params.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<IRecord>> Get$safeprojectname$(QueryParams<$safeprojectname$Entrity> queryParams);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> Update$safeprojectname$(string id, IRecord record);

        /// <summary>
        /// Deletes the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> Delete$safeprojectname$(IEnumerable<string> ids);

        /// <summary>
        /// Deletes the user return error ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<string>> Delete$safeprojectname$ReturnErrorIds(IEnumerable<string> ids);

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> Insert$safeprojectname$(IEnumerable<$safeprojectname$Entrity> users);

        /// <summary>
        /// Inserts the and get.
        /// </summary>
        /// <param name="userEntrity">The user entrity.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<$safeprojectname$Entrity> InsertAndGet($safeprojectname$Entrity userEntrity);
    }
}