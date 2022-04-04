namespace Domain;

public class Buchhaltung : List<Reise>
{
  public void ErfasseReise(DateTime anfang, DateTime ende)
  {
    ReiseMussDieEinzigeImZeitraumSein(anfang, ende);

    Add(new Reise(anfang, ende));
  }

  void ReiseMussDieEinzigeImZeitraumSein(DateTime anfang, DateTime ende)
  {
    if (this.Any(reise => reise.Anfang <= anfang && reise.Ende >= ende))
    {
      throw new ZuEinemZeitpunktDarfNurEineReiseErfasstWerden();
    }
  }
}
