namespace Domain;

public interface ISystemClock
{
  DateTime Now => DateTime.Now;
}
