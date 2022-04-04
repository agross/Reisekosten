namespace Domain.Pauschalen;

class PauschaleAußerhalbDerEu : IPauschalenStrategy
{
  public decimal Berechnen(IEnumerable<(DateTime anfang, DateTime ende)> tageweise)
  {
    return tageweise.Sum(_ => 100m);
  }
}
