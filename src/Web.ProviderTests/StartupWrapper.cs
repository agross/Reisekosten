using System.Linq;

using Application;

using Domain.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Web.ProviderTests.MiddlewareForTesting;
using Web.ProviderTests.Persistence;
using Web.ProviderTests.Services;

namespace Web.ProviderTests;

class StartupWrapper
{
  readonly Startup _proxy;

  public StartupWrapper(IConfiguration configuration)
  {
    _proxy = new Startup(configuration);
  }

  public void ConfigureServices(IServiceCollection services)
  {
    _proxy.ConfigureServices(services);

    // Kick out Marten.
    var marten =
      services.FirstOrDefault(d => d.ServiceType == typeof(IHostedService) &&
                                   d.ImplementationType?.Name == "MartenActivator");

    if (marten != null)
    {
      services.Remove(marten);
    }

    services.Where(d => d.ServiceType == typeof(IBuchhaltungRepository))
            .ToList()
            .ForEach(x => services.Remove(x));

    // Override default database-backed persistence.
    services.AddSingleton<IBuchhaltungRepository, InMemoryBuchhaltungRepository>();

    services.AddSingleton<ISystemClock, FakeSystemClock>();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    app.UseMiddleware<ProviderStateMiddleware>();

    _proxy.Configure(app, env);
  }
}
