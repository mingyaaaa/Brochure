using Brochure.Abstract;
using PluginTemplate.Entrities;

namespace PluginTemplate.Dals
{
    public interface I$safeprojectname$Dal
    {
 
        ValueTask<IEnumerable<$safeprojectname$Entrity>> Get$safeprojectname$(IEnumerable<string> ids);


        ValueTask<IEnumerable<IRecord>> Get$safeprojectname$(QueryParams<$safeprojectname$Entrity> queryParams);


        ValueTask<int> Update$safeprojectname$(string id, IRecord record);


        ValueTask<int> Delete$safeprojectname$(IEnumerable<string> ids);


        ValueTask<IEnumerable<string>> Delete$safeprojectname$ReturnErrorIds(IEnumerable<string> ids);


        ValueTask<int> Insert$safeprojectname$(IEnumerable<$safeprojectname$Entrity> users);


        ValueTask<$safeprojectname$Entrity> InsertAndGet($safeprojectname$Entrity userEntrity);
    }
}