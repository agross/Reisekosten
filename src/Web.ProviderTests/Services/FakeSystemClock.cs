using System;

using Domain.Services;

namespace Web.ProviderTests.Services;

public class FakeSystemClock : ISystemClock
{
  public DateTime Now => DateTime.MinValue;
}
