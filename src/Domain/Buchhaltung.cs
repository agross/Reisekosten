namespace Domain;

public class Buchhaltung
{
  readonly List<Reise> _reisen = new();

  public void ErfasseReise(Reisekostenformular formular, ISystemClock clock)
  {
    ReisenDesVorjahresMüssenBis10JanuarErfasstWerden(formular.Ende, clock);
    ReiseMussDieEinzigeImZeitraumSein(formular.Anfang, formular.Ende);

    _reisen.Add(new Reise(formular));
  }

  void ReiseMussDieEinzigeImZeitraumSein(DateTime anfang, DateTime ende)
  {
    if (_reisen.Any(reise => reise.Formular.Anfang <= anfang &&
                             reise.Formular.Ende >= ende))
    {
      throw new ZuEinemZeitpunktDarfNurEineReiseErfasstWerden();
    }
  }

  void ReisenDesVorjahresMüssenBis10JanuarErfasstWerden(DateTime ende, ISystemClock clock)
  {
    var einzureichenBis = new DateTime(ende.AddYears(1).Year, 1, 10, 23, 59, 59);

    if (clock.Now > einzureichenBis)
    {
      throw new ReiseWurdeZuSpätEingereicht();
    }
  }

  public Bericht ErzeugeBericht(ITranslateCitiesToEuCountries geo)
  {
    var pauschalen = _reisen.Select(reise => new ReisePauschale(reise.Formular.Anfang,
                                                                reise.Formular.Ende,
                                                                reise.Formular.Zielort,
                                                                reise.Formular.Grund,
                                                                reise.Pauschale(geo)));

    return new Bericht(pauschalen);
  }
}
