using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.User.Abstract.RequestModel;

namespace Brochure.User.Services.Interfaces
{
    /// <summary>
    /// The user dal.
    /// </summary>
    public interface IUserDal
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserModel>> GetUsers(IEnumerable<string> ids);

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
        /// Deletes the user return error ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<string>> DeleteUserReturnErrorIds(IEnumerable<string> ids);

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserModel>> InsertUsers(IEnumerable<UserModel> users);
    }
}