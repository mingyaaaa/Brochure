using Brochure.Core.PluginsDI;
using Brochure.Server.Main;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Host.UseServiceProviderFactory(new PluginServiceProviderFactory());
var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();