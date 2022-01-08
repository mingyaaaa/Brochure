using Grpc.Core;
using Proto.Organization.Grpc;
using System.Threading.Tasks;
using static Proto.Organization.Grpc.OrganizationService;

namespace Brochure.Organization.Services
{
    /// <summary>
    /// The grpc org services.
    /// </summary>
    public class GrpcOrgServices:OrganizationServiceBase
    {
        /// <summary>
        /// Deletes the organization.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override Task<FailIdsResponse> DeleteOrganization(OrganizationRequest request, ServerCallContext context)
        {
            return base.DeleteOrganization(request, context);
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override Task<OrganizationResponse> GetOrganization(OrganizationRequest request, ServerCallContext context)
        {
            return base.GetOrganization(request, context);
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override Task<OrganizationResponse> Insert(OrganizationRequest request, ServerCallContext context)
        {
            return base.Insert(request, context);
        }

        /// <summary>
        /// Inserts the muti.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override Task<OrganizationResponse> InsertMuti(MutiOrganizationRequest request, ServerCallContext context)
        {
            return base.InsertMuti(request, context);
        }

        /// <summary>
        /// Updates the organization.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override Task<FailIdsResponse> UpdateOrganization(OrganizationRequest request, ServerCallContext context)
        {
            return base.UpdateOrganization(request, context);
        }
    }
}
