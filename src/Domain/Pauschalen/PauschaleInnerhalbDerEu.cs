namespace Domain.Pauschalen;

class PauschaleInnerhalbDerEu : IPauschalenStrategy
{
  static readonly Dictionary<Func<TimeSpan, bool>, decimal> ZeitZuPauschalen = new()
  {
    { ts => ts >= TimeSpan.FromHours(24), 24 },
    { ts => ts >= TimeSpan.FromHours(12), 12 },
    { ts => ts >= TimeSpan.FromHours(8), 6 },
  };

  public decimal Berechnen(IEnumerable<(DateTime anfang, DateTime ende)> tageweise)
  {
    return tageweise.Aggregate(0m,
                               (pauschale, tag) =>
                               {
                                 var match = ZeitZuPauschalen.FirstOrDefault(kvp => kvp.Key(Dauer(tag)));

                                 return pauschale + match.Value;
                               });
  }

  static TimeSpan Dauer((DateTime anfang, DateTime ende) tag)
    => tag.ende - tag.anfang;
}
