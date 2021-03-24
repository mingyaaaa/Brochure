using Brochure.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.User.Abstract
{
    /// <summary>
    /// The user service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserServiceModel>> GetUsers();

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserServiceModel>> GetUsers(IEnumerable<string> ids);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> UpdateUser(string id, IRecord record);

        /// <summary>
        /// Deletes the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> DeleteUsers(IEnumerable<string> ids);

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserServiceModel>> InsertUsers(IEnumerable<UserServiceModel> users);
    }
}
