using System;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Authority
{
	public class AuthorityPlugin : Plugins
	{
		public AuthorityPlugin (AssemblyLoadContext assemblyContext) : base (assemblyContext) { }

		public override Task<bool> StartingAsync ()
		{
			ServiceDescriptors.AddIdentityServer ().AddJwtBearerClientAuthentication ();
			return base.StartingAsync ();
		}
	}
}