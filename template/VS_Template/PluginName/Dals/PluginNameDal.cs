using Brochure.Abstract;
using PluginTemplate.Entrities;
using PluginTemplate.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTemplate.Dals
{
    internal class $safeprojectname$Dal : I$safeprojectname$Dal
    {
        private readonly I$safeprojectname$Repository _repository;
        private readonly IObjectFactory _objectFactory;

        public $safeprojectname$Dal(I$safeprojectname$Repository repository, IObjectFactory objectFactory)
        {
            _repository = repository;
            _objectFactory = objectFactory;
        }

        public async ValueTask<int> Delete$safeprojectname$(IEnumerable<string> ids)
        {
            var r = await _repository.DeleteManyAsync(ids);
            return r;
        }

        public async ValueTask<IEnumerable<string>> Delete$safeprojectname$ReturnErrorIds(IEnumerable<string> ids)
        {
            var r_ids = await _repository.DeleteManyReturnErrorAsync(ids);
            return r_ids;
        }

        public async ValueTask<IEnumerable<$safeprojectname$Entrity>> Get$safeprojectname$(IEnumerable<string> ids)
        {
            var idsList = ids.ToList();
            var count = idsList.Count;
            if (count == 0)
                return new List<$safeprojectname$Entrity>();
            var entrity = await _repository.ListAsync(ids);
            return entrity;
        }

        public async ValueTask<IEnumerable<IRecord>> Get$safeprojectname$(QueryParams<$safeprojectname$Entrity> queryParams)
        {
            var entrity = await _repository.ListAsync(queryParams);
            return entrity;
        }

        public async ValueTask<$safeprojectname$Entrity> InsertAndGet($safeprojectname$Entrity userEntrity)
        {
            var r = await _repository.InsertAndGetAsync(userEntrity);
            return r;
        }

        public async ValueTask<int> Insert$safeprojectname$(IEnumerable<$safeprojectname$Entrity> users)
        {
            var r = await _repository.InsertAsync(users);
            return r;
        }

        public async ValueTask<int> Update$safeprojectname$(string id, IRecord record)
        {
            var r_entiry = _objectFactory.Create<$safeprojectname$Entrity>(record);
            var r = await _repository.UpdateAsync(id, r_entiry);
            return r;
        }
    }
}