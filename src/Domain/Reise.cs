namespace Domain;

public record Reise(Reisekostenformular Formular)
{
  public decimal Pauschale(ITranslateCitiesToEuCountries geo)
  {
    return PauschaleInnerhalbDerEu.Berechnen(Tageweise());
  }

  IEnumerable<(DateTime anfang, DateTime ende)> Tageweise()
  {
    var anfang = Formular.Anfang;

    do
    {
      var mitternacht = new DateTime(anfang.AddDays(1).Year,
                                     anfang.AddDays(1).Month,
                                     anfang.AddDays(1).Day,
                                     00,
                                     00,
                                     00);

      if (mitternacht > Formular.Ende)
      {
        mitternacht = Formular.Ende;
      }

      yield return (anfang, mitternacht);

      anfang = mitternacht;
    }
    while (anfang < Formular.Ende);
  }
}
