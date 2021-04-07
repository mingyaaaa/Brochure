using AutoFixture;
using Brochure.User.Abstract.RequestModel;
using Brochure.User.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;

namespace Brochure.User.Test
{
    [TestClass]
    public class TestUserAdd : BaseTest
    {

        public override void ConfigureService(IServiceCollection services)
        {
            services.AddHttpApi<IUserWebApi>().ConfigureHttpApi(t =>
            {
                t.HttpHost = new Uri(Host);
            });
            base.ConfigureService(services);
        }
        [TestMethod("测试添加人员")]
        public async Task TestAdd()
        {
            var api = this.Service.GetService<IUserWebApi>();
            var req = Fixture.Create<ReqAddUserModel>();
            var rsp = await api.Add(req);

            Assert.AreEqual(req.IdCard, rsp.IdCard);
            Assert.AreEqual(req.Name, rsp.Name);

        }
    }
}
