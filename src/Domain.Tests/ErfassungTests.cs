using FakeItEasy;

using FluentAssertions;

using NUnit.Framework;

namespace Domain.Tests;

public record Reisekostenformular(DateTime Anfang, DateTime Ende, string Zielort, string Grund);

[TestFixture]
public class ErfassungTests : ISystemClock
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

    Erfasse(new Reisekostenformular(anfang, ende, zielort, grund), this);
  }

  [Test]
  public void Soll_Reisen_ablehnen_deren_Ende_vor_dem_Beginn_liegt()
  {
    var anfang = DateTime.MaxValue;
    var ende = DateTime.MinValue;
    var zielort = "egal";
    var grund = "egal";

    FluentActions.Invoking(() => Erfasse(new Reisekostenformular(anfang, ende, zielort, grund), this))
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

    Erfasse(new Reisekostenformular(anfang, ende, zielort, grund), this);

    FluentActions.Invoking(() => Erfasse(new Reisekostenformular(anfang, ende, zielort, grund), this))
                 .Should()
                 .Throw<ZuEinemZeitpunktDarfNurEineReiseErfasstWerden>();
  }

  [Test]
  public void Soll_Reise_für_2021_ab_11_Januar_2022_ablehnen()
  {
    var anfang = DateTime.MinValue;
    var ende = new DateTime(2021, 12, 31);
    var zielort = "egal";
    var grund = "egal";

    ISystemClock clock = new Januar_11_2022_Clock();

    FluentActions.Invoking(() => Erfasse(new Reisekostenformular(anfang, ende, zielort, grund), clock))
                 .Should()
                 .Throw<ReiseWurdeZuSpätEingereicht>();
  }

  [Test]
  public void Soll_Reise_für_2021_bis_zum_10_Januar_2022_erfassen()
  {
    var anfang = DateTime.MinValue;
    var ende = new DateTime(2021, 12, 31);
    var zielort = "egal";
    var grund = "egal";

    var clock = A.Fake<ISystemClock>();
    A.CallTo(() => clock.Now)
     .Returns(new DateTime(2022, 1, 10));

    Erfasse(new Reisekostenformular(anfang, ende, zielort, grund), clock);
  }

  class Januar_11_2022_Clock : ISystemClock
  {
    public DateTime Now => new(2022, 1, 11);
  }

  Buchhaltung _buchhaltung = new();

  void Erfasse(Reisekostenformular formular, ISystemClock clock)
  {
    _buchhaltung.ErfasseReise(formular.Anfang, formular.Ende, clock);
  }
}
