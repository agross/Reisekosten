using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Application;

using Domain.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

using Newtonsoft.Json;

using PactNet;

using Web.ProviderTests.Persistence;
using Web.ProviderTests.Services;

namespace Web.ProviderTests.MiddlewareForTesting;

class ProviderStateMiddleware
{
  readonly RequestDelegate _next;
  readonly InMemoryBuchhaltungRepository _buchhaltung;
  readonly Dictionary<string, Action> _providerStates;

  public ProviderStateMiddleware(RequestDelegate next,
                                 IBuchhaltungRepository buchhaltung)
  {
    _next = next;
    _buchhaltung = (InMemoryBuchhaltungRepository) buchhaltung;

    _providerStates = new Dictionary<string, Action>
    {
      { "Buchhaltung ohne Einträge", BuchhaltungOhneEinträge },
      { "Buchhaltung mit 2 Einträgen", BuchhaltungMit2Einträgen },
    };
  }

  void BuchhaltungOhneEinträge()
  {
    _buchhaltung.SetState(new Buchhaltung { Id = 42 });
  }

  void BuchhaltungMit2Einträgen()
  {
    var b = new Buchhaltung { Id = 42 };
    b.ErfasseReise(new Reisekostenformular(DateTime.MinValue,
                                           DateTime.MinValue.AddDays(1),
                                           "Zielort 1",
                                           "Grund 1"),
                   new FakeSystemClock());

    b.ErfasseReise(new Reisekostenformular(DateTime.MinValue.AddDays(2),
                                           DateTime.MinValue.AddDays(3),
                                           "Zielort 2",
                                           "Grund 2"),
                   new FakeSystemClock());

    _buchhaltung.SetState(b);
  }

  public async Task Invoke(HttpContext context)
  {
    if (context.Request.Path.StartsWithSegments("/provider-states"))
    {
      await HandleProviderStatesRequest(context);
      await context.Response.WriteAsync(string.Empty);
    }
    else
    {
      await _next(context);
    }
  }

  async Task HandleProviderStatesRequest(HttpContext context)
  {
    context.Response.StatusCode = (int) HttpStatusCode.OK;

    if (!string.Equals(context.Request.Method,
                       HttpMethod.Post.ToString(),
                       StringComparison.CurrentCultureIgnoreCase))
    {
      context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

      return;
    }

    string jsonRequestBody;
    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
    {
      jsonRequestBody = await reader.ReadToEndAsync();
    }

    var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

    // A null or empty provider state key must be handled.
    if (providerState != null && !string.IsNullOrEmpty(providerState.State))
    {
      _providerStates[providerState.State].Invoke();
    }
  }
}
