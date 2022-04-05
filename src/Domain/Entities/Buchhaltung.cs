using Domain.Errors;
using Domain.Services;

namespace Domain.Entities;

public class Buchhaltung
{
  List<Reise> _reisen = new();

  public int Id { get; set; }

  public IEnumerable<Reise> Reisen
  {
    get => _reisen;
    private set => _reisen = new List<Reise>(value);
  }

  public void ErfasseReise(Reisekostenformular formular, ISystemClock clock)
  {
    ReisenDesVorjahresMüssenBis10JanuarErfasstWerden(formular.Ende, clock);
    ReiseMussDieEinzigeImZeitraumSein(formular.Anfang, formular.Ende);

    _reisen.Add(new Reise(formular));
  }

  void ReiseMussDieEinzigeImZeitraumSein(DateTime anfang, DateTime ende)
  {
    if (Reisen.Any(reise => reise.Formular.Anfang <= anfang &&
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
    var pauschalen = Reisen.Select(reise => new ReisePauschale(reise.Formular.Anfang,
                                                               reise.Formular.Ende,
                                                               reise.Formular.Zielort,
                                                               reise.Formular.Grund,
                                                               reise.Pauschale(geo)));

    return new Bericht(pauschalen);
  }
}
