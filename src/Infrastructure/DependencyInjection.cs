using Application;

using Domain.Services;

using Infrastructure.Persistence;
using Infrastructure.Services;

using Marten;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
  public static void AddInfrastructure(this IServiceCollection services,
                                       IConfiguration configuration)
  {
    services.AddSingleton<ISystemClock, SystemClock>();
    services.AddSingleton<ITranslateCitiesToEuCountries, FakeLocationToEuMapper>();
    services.AddScoped<IBuchhaltungRepository, BuchhaltungRepository>();

    services.AddMarten(options =>
            {
              options.Connection(configuration.GetConnectionString("Postgres"));
              options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);
            })
            .InitializeWith(new InitialData(InitialDatasets.Buchhaltung));
  }
}
