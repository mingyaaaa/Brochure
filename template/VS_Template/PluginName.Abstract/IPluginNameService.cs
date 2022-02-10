using Brochure.Abstract;

namespace Plugin.Abstract
{
    public interface I$safeprojectname$Service
    {
        ValueTask<IEnumerable<$safeprojectname$ServiceModel>> Get$safeprojectname$();

        ValueTask<IEnumerable<$safeprojectname$ServiceModel>> GetUsers(IEnumerable<string> ids);

        ValueTask<int> Update$safeprojectname$(string id, IRecord record);

        ValueTask<int> Delete$safeprojectname$(IEnumerable<string> ids);

        ValueTask<IEnumerable<$safeprojectname$ServiceModel>> Insert$safeprojectname$(IEnumerable<$safeprojectname$ServiceModel> users);
    }
}