using Microsoft.AspNetCore.Builder;
namespace Brochure.Authority.Abstract.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBrochureAuthorization (this IApplicationBuilder app)
        {
            return app.UseAuthorization ();
        }
    }
}