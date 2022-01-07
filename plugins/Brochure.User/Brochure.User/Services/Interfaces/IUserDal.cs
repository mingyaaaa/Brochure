using Brochure.Abstract;
using Brochure.User.Entrities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        ValueTask<IEnumerable<UserEntrity>> GetUsers(IEnumerable<string> ids);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="queryParams">The query params.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<IRecord>> GetUsers(QueryParams<UserEntrity> queryParams);

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
        ValueTask<int> InsertUsers(IEnumerable<UserEntrity> users);

        /// <summary>
        /// Inserts the and get.
        /// </summary>
        /// <param name="userEntrity">The user entrity.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<UserEntrity> InsertAndGet(UserEntrity userEntrity);
    }
}