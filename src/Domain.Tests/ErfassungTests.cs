using FluentAssertions;

using NUnit.Framework;

namespace Domain.Tests;

[TestFixture]
public class ErfassungTests
{
  [Test]
  public void Soll_Reisen_mit_Anfang_Ende_Zielort_und_Reisegrund_akzeptieren()
  {
    var anfang = DateTime.MinValue;
    var ende = DateTime.MinValue;
    var zielort = "egal";
    var grund = "egal";

    Erfasse(anfang, ende, zielort, grund);
  }

  [Test]
  public void Soll_Reisen_ablehnen_deren_Ende_vor_dem_Beginn_liegt()
  {
    var anfang = DateTime.MaxValue;
    var ende = DateTime.MinValue;
    var zielort = "egal";
    var grund = "egal";

    FluentActions.Invoking(() => Erfasse(anfang, ende, zielort, grund))
                 .Should()
                 .Throw<EndeDerReiseMussNachReisebeginnLiegen>();
  }

  [Test]
  public void Soll_zu_einem_Zeitpunkt_nur_eine_Reise_akzeptieren()
  {
    var anfang = DateTime.MinValue;
    var ende = DateTime.MaxValue;
    var zielort = "egal";
    var grund = "egal";

    Erfasse(anfang, ende, zielort, grund);

    FluentActions.Invoking(() => Erfasse(anfang, ende, zielort, grund))
                 .Should()
                 .Throw<ZuEinemZeitpunktDarfNurEineReiseErfasstWerden>();
  }

  readonly Buchhaltung _buchhaltung = new();

  class Buchhaltung : List<Reise>
  {
  }

  void Erfasse(DateTime anfang, DateTime ende, string zielort, string grund)
  {
    if (ende < anfang)
    {
      throw new EndeDerReiseMussNachReisebeginnLiegen();
    }

    ErfasseInBuchhaltung(_buchhaltung, anfang, ende);
  }

  void ErfasseInBuchhaltung(Buchhaltung buchhaltung, DateTime anfang, DateTime ende)
  {
    ReiseMussDieEinzigeImZeitraumSein(buchhaltung, anfang, ende);

    buchhaltung.Add(new Reise(anfang, ende));
  }

  void ReiseMussDieEinzigeImZeitraumSein(Buchhaltung buchhaltung, DateTime anfang, DateTime ende)
  {
    if (buchhaltung.Any(reise => reise.Anfang <= anfang && reise.Ende >= ende))
    {
      throw new ZuEinemZeitpunktDarfNurEineReiseErfasstWerden();
    }
  }
}

record Reise(DateTime Anfang, DateTime Ende);

public class EndeDerReiseMussNachReisebeginnLiegen : Exception
{
}

public class ZuEinemZeitpunktDarfNurEineReiseErfasstWerden : Exception
{
}
