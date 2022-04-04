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
      return Tageweise(Formular)
        .Aggregate(0m,
                   (pauschale1, tag) =>
                   {
                     foreach (var (predicate, summe) in ZeitZuPauschalen)
                     {
                       if (predicate(Dauer(tag)))
                       {
                         return pauschale1 + summe;
                       }
                     }

                     return 0;
                   });
    }
  }

  IEnumerable<(DateTime anfang, DateTime ende)> Tageweise(Reisekostenformular formular)
  {
    var anfang = formular.Anfang;

    do
    {
      var mitternacht = new DateTime(anfang.AddDays(1).Year,
                                     anfang.AddDays(1).Month,
                                     anfang.AddDays(1).Day,
                                     00,
                                     00,
                                     00);

      if (mitternacht > formular.Ende)
      {
        mitternacht = formular.Ende;
      }

      yield return (anfang, mitternacht);

      anfang = mitternacht;
    }
    while (anfang < formular.Ende);
  }

  static TimeSpan Dauer((DateTime anfang, DateTime ende) tag)
    => tag.ende - tag.anfang;
}
