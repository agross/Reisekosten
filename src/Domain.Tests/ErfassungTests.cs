using FluentAssertions;

using NUnit.Framework;

namespace Domain.Tests;

[TestFixture]
public class ErfassungTests
{
  [SetUp]
  public void SetUp()
  {
    _buchhaltung = new Buchhaltung();
  }

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

  Buchhaltung _buchhaltung = new();

  void Erfasse(DateTime anfang, DateTime ende, string zielort, string grund)
  {
    if (ende < anfang)
    {
      throw new EndeDerReiseMussNachReisebeginnLiegen();
    }

    _buchhaltung.ErfasseReise(anfang, ende);
  }
}
