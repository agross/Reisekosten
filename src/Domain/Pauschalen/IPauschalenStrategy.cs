namespace Domain.Pauschalen;

interface IPauschalenStrategy
{
  decimal Berechnen(IEnumerable<(DateTime anfang, DateTime ende)> tageweise);
}
