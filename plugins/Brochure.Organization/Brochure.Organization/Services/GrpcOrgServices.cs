using Grpc.Core;
using Proto.Organization.Grpc;
using System.Threading.Tasks;
using static Proto.Organization.Grpc.OrganizationService;

namespace Brochure.Organization.Services
{
    public class GrpcOrgServices:OrganizationServiceBase
    {
        public override Task<FailIdsResponse> DeleteOrganization(OrganizationRequest request, ServerCallContext context)
        {
            return base.DeleteOrganization(request, context);
        }

        public override Task<OrganizationResponse> GetOrganization(OrganizationRequest request, ServerCallContext context)
        {
            return base.GetOrganization(request, context);
        }

        public override Task<OrganizationResponse> Insert(OrganizationRequest request, ServerCallContext context)
        {
            return base.Insert(request, context);
        }

        public override Task<OrganizationResponse> InsertMuti(MutiOrganizationRequest request, ServerCallContext context)
        {
            return base.InsertMuti(request, context);
        }

        public override Task<FailIdsResponse> UpdateOrganization(OrganizationRequest request, ServerCallContext context)
        {
            return base.UpdateOrganization(request, context);
        }
    }
}
