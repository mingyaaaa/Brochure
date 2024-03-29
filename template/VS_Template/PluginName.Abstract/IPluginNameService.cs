﻿using Brochure.Abstract;

namespace Plugin.Abstract
{
    public interface I$ext_safeprojectname$Service
    {
        ValueTask<IEnumerable<$ext_safeprojectname$ServiceModel>> Get$ext_safeprojectname$();

        ValueTask<IEnumerable<$ext_safeprojectname$ServiceModel>> GetUsers(IEnumerable<string> ids);

        ValueTask<int> Update$ext_safeprojectname$(string id, IRecord record);

        ValueTask<int> Delete$ext_safeprojectname$(IEnumerable<string> ids);

        ValueTask<IEnumerable<$ext_safeprojectname$ServiceModel>> Insert$ext_safeprojectname$(IEnumerable<$ext_safeprojectname$ServiceModel> users);
    }
}