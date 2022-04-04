using FluentAssertions;

using NUnit.Framework;

namespace Domain.Tests;

[TestFixture]
public class AuswertungTests : ISystemClock
{
  public DateTime Now => DateTime.MinValue;

  [Test]
  public void Soll_keine_Pauschale_für_Reisen_unter_8_Stunden_erhalten()
  {
    var anfang = DateTime.MinValue;
    var ende = anfang.AddHours(7);
    var zielort = "egal";
    var grund = "egal";
    var formular = new Reisekostenformular(anfang, ende, zielort, grund);

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(formular, this);

    buchhaltung.First()
               .Pauschale
               .Should()
               .Be(0);
  }

  [Test]
  public void Soll_6_EUR_Pauschale_für_Reisen_ab_8_Stunden_erhalten()
  {
    var anfang = DateTime.MinValue;
    var ende = anfang.AddHours(8);
    var zielort = "egal";
    var grund = "egal";
    var formular = new Reisekostenformular(anfang, ende, zielort, grund);

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(formular, this);

    buchhaltung.First()
               .Pauschale
               .Should()
               .Be(6);
  }

  [Test]
  public void Soll_12_EUR_Pauschale_für_Reisen_ab_12_Stunden_erhalten()
  {
    var anfang = DateTime.MinValue;
    var ende = anfang.AddHours(12);
    var zielort = "egal";
    var grund = "egal";
    var formular = new Reisekostenformular(anfang, ende, zielort, grund);

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(formular, this);

    buchhaltung.First()
               .Pauschale
               .Should()
               .Be(12);
  }

  [Test]
  public void Soll_24_EUR_Pauschale_für_Reisen_ab_24_Stunden_erhalten()
  {
    var anfang = DateTime.MinValue;
    var ende = anfang.AddHours(24);
    var zielort = "egal";
    var grund = "egal";
    var formular = new Reisekostenformular(anfang, ende, zielort, grund);

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(formular, this);

    buchhaltung.First()
               .Pauschale
               .Should()
               .Be(24);
  }
}
