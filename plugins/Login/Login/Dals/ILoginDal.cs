using Brochure.Abstract;
using PluginTemplate.Entrities;

namespace PluginTemplate.Dals
{
    public interface ILoginDal
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<LoginEntrity>> GetLogin(IEnumerable<string> ids);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="queryParams">The query params.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<IRecord>> GetLogin(QueryParams<LoginEntrity> queryParams);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> UpdateLogin(string id, IRecord record);

        /// <summary>
        /// Deletes the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> DeleteLogin(IEnumerable<string> ids);

        /// <summary>
        /// Deletes the user return error ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<string>> DeleteLoginReturnErrorIds(IEnumerable<string> ids);

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> InsertLogin(IEnumerable<LoginEntrity> users);

        /// <summary>
        /// Inserts the and get.
        /// </summary>
        /// <param name="userEntrity">The user entrity.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<LoginEntrity> InsertAndGet(LoginEntrity userEntrity);
    }
}