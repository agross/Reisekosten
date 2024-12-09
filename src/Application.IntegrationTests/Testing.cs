using Domain.Services;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

using Marten;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
  internal static IServiceScopeFactory ScopeFactory = null!;
  IConfigurationRoot _configuration = null!;
  IContainerService _postgres = null!;

  [OneTimeSetUp]
  public void RunBeforeAllTests()
  {
    RunDatabaseWithDocker();

    var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", true, true)
                  .AddEnvironmentVariables();

    _configuration = builder.Build();

    var services = new ServiceCollection();

    services.AddApplication();

    services.AddMarten(options =>
    {
      options.Connection(_configuration.GetConnectionString("Postgres"));
      options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);
    });

    services.AddScoped<IBuchhaltungRepository, BuchhaltungRepository>();
    services.AddSingleton<ISystemClock, SystemClock>();
    services.AddSingleton<ITranslateCitiesToEuCountries, AlwaysInsideEu>();

    ScopeFactory = services.BuildServiceProvider()
                           .GetRequiredService<IServiceScopeFactory>();
  }

  void RunDatabaseWithDocker()
  {
    _postgres = new Builder()
                .UseContainer()
                .UseImage("postgres:alpine")
                .ExposePort(5432, 5432)
                .WaitForPort("5432/tcp", TimeSpan.FromSeconds(30))
                .WithEnvironment("POSTGRES_DB=ReisekostenTestDb",
                  "POSTGRES_USER=postgres",
                  "POSTGRES_PASSWORD=secret-123")
                .RemoveVolumesOnDispose()
                .UseHealthCheck("\"pg_isready -U postgres\"", startPeriod: "1s", interval: "1s")
                .WaitForHealthy()
                .Build()
                .Start();
  }

  [OneTimeTearDown]
  public void RunAfterAllTests()
  {
    _postgres?.Dispose();
  }

  public static void SetBuchhaltungIdForTesting(int buchhaltungId,
                                                IServiceScope scope)
  {
    var repo = scope.ServiceProvider.GetRequiredService<IBuchhaltungRepository>();

    ((BuchhaltungRepository) repo).SetBuchhaltungIdForTesting(buchhaltungId);
  }

  public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
                                                           IServiceScope scope)
  {
    var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

    return await mediator.Send(request);
  }

  public static async Task SendAsync(IRequest request,
                                     IServiceScope scope)
  {
    var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

    await mediator.Send(request);
  }

  public static async Task AddAsync<T>(T obj) where T : notnull
  {
    using var scope = ScopeFactory.CreateScope();

    var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

    session.Store(obj);

    await session.SaveChangesAsync();
  }

  public static async Task<T?> LoadAsync<T>(int id) where T : notnull
  {
    using var scope = ScopeFactory.CreateScope();

    var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

    return await session.LoadAsync<T>(id);
  }
}
