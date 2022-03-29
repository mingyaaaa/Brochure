using Brochure.Abstract;

namespace Plugin.Abstract
{
    public interface ILoginService
    {
        ValueTask<IEnumerable<LoginServiceModel>> GetLogin();

        ValueTask<IEnumerable<LoginServiceModel>> GetUsers(IEnumerable<string> ids);

        ValueTask<int> UpdateLogin(string id, IRecord record);

        ValueTask<int> DeleteLogin(IEnumerable<string> ids);

        ValueTask<IEnumerable<LoginServiceModel>> InsertLogin(IEnumerable<LoginServiceModel> users);
    }
}