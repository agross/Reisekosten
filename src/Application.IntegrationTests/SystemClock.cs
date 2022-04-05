using Domain.Services;

namespace Application.IntegrationTests;

public class SystemClock : ISystemClock
{
  public DateTime Now => DateTime.MinValue;
}
