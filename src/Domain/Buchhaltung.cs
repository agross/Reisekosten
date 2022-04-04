namespace Domain;

public class Buchhaltung : List<Reise>
{
  public void ErfasseReise(DateTime anfang, DateTime ende, ISystemClock clock)
  {
    EndeDerReiseMussNachReiseBeginnLiegen(anfang, ende);
    ReisenDesVorjahresMüssenBis10JanuarErfasstWerden(ende, clock);
    ReiseMussDieEinzigeImZeitraumSein(anfang, ende);

    Add(new Reise(anfang, ende));
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
    if (this.Any(reise => reise.Anfang <= anfang && reise.Ende >= ende))
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
}
