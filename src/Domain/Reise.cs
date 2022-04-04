namespace Domain;

public record Reise(Reisekostenformular Formular)
{
  static readonly Dictionary<Func<TimeSpan, bool>, decimal> ZeitZuPauschalen = new()
  {
    { ts => ts >= TimeSpan.FromHours(24), 24 },
    { ts => ts >= TimeSpan.FromHours(12), 12 },
    { ts => ts >= TimeSpan.FromHours(8), 6 },
  };

  public decimal Pauschale
  {
    get
    {
      foreach (var (predicate, pauschale) in ZeitZuPauschalen)
      {
        if (predicate(Dauer))
        {
          return pauschale;
        }
      }

      return 0;
    }
  }

  TimeSpan Dauer => Formular.Ende - Formular.Anfang;
}
