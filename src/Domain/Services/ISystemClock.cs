namespace Domain.Services;

public interface ISystemClock
{
  DateTime Now => DateTime.Now;
}
