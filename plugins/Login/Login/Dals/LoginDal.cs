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
    internal class LoginDal : ILoginDal
    {
        private readonly ILoginRepository _repository;
        private readonly IObjectFactory _objectFactory;

        public LoginDal(ILoginRepository repository, IObjectFactory objectFactory)
        {
            _repository = repository;
            _objectFactory = objectFactory;
        }

        public async ValueTask<int> DeleteLogin(IEnumerable<string> ids)
        {
            var r = await _repository.DeleteManyAsync(ids);
            return r;
        }

        public async ValueTask<IEnumerable<string>> DeleteLoginReturnErrorIds(IEnumerable<string> ids)
        {
            var r_ids = await _repository.DeleteManyReturnErrorAsync(ids);
            return r_ids;
        }

        public async ValueTask<IEnumerable<LoginEntrity>> GetLogin(IEnumerable<string> ids)
        {
            var idsList = ids.ToList();
            var count = idsList.Count;
            if (count == 0)
                return new List<LoginEntrity>();
            var entrity = await _repository.ListAsync(ids);
            return entrity;
        }

        public async ValueTask<IEnumerable<IRecord>> GetLogin(QueryParams<LoginEntrity> queryParams)
        {
            var entrity = await _repository.ListAsync(queryParams);
            return entrity;
        }

        public async ValueTask<LoginEntrity> InsertAndGet(LoginEntrity userEntrity)
        {
            var r = await _repository.InsertAndGetAsync(userEntrity);
            return r;
        }

        public async ValueTask<int> InsertLogin(IEnumerable<LoginEntrity> users)
        {
            var r = await _repository.InsertAsync(users);
            return r;
        }

        public async ValueTask<int> UpdateLogin(string id, IRecord record)
        {
            var r_entiry = _objectFactory.Create<LoginEntrity>(record);
            var r = await _repository.UpdateAsync(id, r_entiry);
            return r;
        }
    }
}