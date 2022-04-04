using System.Collections;

namespace Domain;

public class Buchhaltung : IEnumerable<Reise>
{
  readonly List<Reise> _reisen = new();

  public void ErfasseReise(Reisekostenformular formular, ISystemClock clock)
  {
    EndeDerReiseMussNachReiseBeginnLiegen(formular.Anfang, formular.Ende);
    ReisenDesVorjahresMüssenBis10JanuarErfasstWerden(formular.Ende, clock);
    ReiseMussDieEinzigeImZeitraumSein(formular.Anfang, formular.Ende);

    _reisen.Add(new Reise(formular));
  }

  void EndeDerReiseMussNachReiseBeginnLiegen(DateTime anfang, DateTime ende)
  {
    if (ende < anfang)
    {
      throw new EndeDerReiseMussNachReisebeginnLiegen();
    }
  }

  void ReiseMussDieEinzigeImZeitraumSein(DateTime anfang, DateTime ende)
  {
    if (this.Any(reise => reise.Formular.Anfang <= anfang &&
                          reise.Formular.Ende >= ende))
    {
      throw new ZuEinemZeitpunktDarfNurEineReiseErfasstWerden();
    }
  }

  void ReisenDesVorjahresMüssenBis10JanuarErfasstWerden(DateTime ende, ISystemClock clock)
  {
    var now = clock.Now;

    if (ende.Year == now.AddYears(-1).Year &&
        now.Day > 10)
    {
      throw new ReiseWurdeZuSpätEingereicht();
    }
  }

  public IEnumerator<Reise> GetEnumerator()
    => _reisen.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator()
    => ((IEnumerable) _reisen).GetEnumerator();
}
