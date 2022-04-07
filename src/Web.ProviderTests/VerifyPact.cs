using System;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using NUnit.Framework;

using PactNet.Verifier;

namespace Web.ProviderTests;

[TestFixture]
public class VerifyPact
{
  static readonly Uri BackendUri = new("http://127.0.0.1:9001");

  IWebHost? _webHost;
  IContainerService? _postgres;

  [SetUp]
  public void RunApplication()
  {
    StartWebApplication();
  }

  [TearDown]
  public void ShutdownApplication()
  {
    _webHost?.Dispose();
    _postgres?.Dispose();
  }

  void StartWebApplication()
  {
    _webHost = WebHost.CreateDefaultBuilder()
                      .UseContentRoot("../../../../Web/")
                      .UseStartup<StartupWrapper>()
                      .UseEnvironment("Development")
                      .UseUrls(BackendUri.ToString())
                      .Build();
    _webHost.Start();
  }

  [Test]
  public void Soll_Pact_verifizieren()
  {
    // Arrange
    var config = new PactVerifierConfig();

    // Act / Assert
    new PactVerifier(config)
      .ServiceProvider("Reisekosten Backend", BackendUri)
      .WithPactBrokerSource(new Uri("https://pact.grossweber.com"),
                            options =>
                            {
                              var sha = Environment.GetEnvironmentVariable("GITHUB_SHA");
                              var branch = Environment.GetEnvironmentVariable("GITHUB_REF_NAME");
                              if (string.IsNullOrWhiteSpace(sha) || string.IsNullOrWhiteSpace(branch))
                              {
                                return;
                              }

                              // https://docs.pact.io/pact_broker/pacticipant_version_numbers
                              var version = $"{sha.Substring(0, 7)}-{branch}";

                              options.PublishResults(version,
                                                     results =>
                                                     {
                                                       // https://github.com/pact-foundation/pact-net/issues/376
                                                       results.BuildUri(BackendUri);
                                                       results.ProviderTags("master");
                                                     });
                            })
      .WithProviderStateUrl(new Uri(BackendUri, "/provider-states"))
      .Verify();
  }
}
