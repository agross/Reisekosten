using Domain.Entities;
using Domain.Services;

using FakeItEasy;

using FluentAssertions;

namespace Domain.Tests;

[TestFixture]
public class AuswertungTests : ISystemClock, ITranslateCitiesToEuCountries
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

    var bericht = Auswerten(buchhaltung, this);

    bericht.Summe
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

    var bericht = Auswerten(buchhaltung, this);

    bericht.Summe
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

    var bericht = Auswerten(buchhaltung, this);

    bericht.Summe
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

    var bericht = Auswerten(buchhaltung, this);

    bericht.Summe
           .Should()
           .Be(24);
  }

  [Test]
  public void Soll_Pauschale_für_mehrtägige_Reise_tageweise_berechnen()
  {
    var anfang = new DateTime(2022, 01, 01, 16, 00, 00);
    var ende = new DateTime(2022, 01, 03, 14, 00, 00);
    var zielort = "egal";
    var grund = "egal";
    var formular = new Reisekostenformular(anfang, ende, zielort, grund);

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(formular, this);

    var bericht = Auswerten(buchhaltung, this);

    bericht.Summe
           .Should()
           .Be(6 + 24 + 12);
  }

  [Test]
  public Task Soll_Reisedaten_im_Bericht_inkludieren()
  {
    var reise1 = new Reisekostenformular(DateTime.MinValue,
                                         DateTime.MinValue.AddDays(1),
                                         "Berlin",
                                         "Republica");
    var reise2 = new Reisekostenformular(DateTime.MinValue.AddDays(1),
                                         DateTime.MinValue.AddDays(2),
                                         "Leipzig",
                                         "Developer Open Space");

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(reise1, this);
    buchhaltung.ErfasseReise(reise2, this);

    var bericht = Auswerten(buchhaltung, this);

    bericht.Should().HaveCount(2);
    bericht.First().Zielort.Should().Be("Berlin");
    bericht.First().Grund.Should().Be("Republica");

    // For more complex data structures, Verify is a good tool.
    VerifierSettings.TreatAsString<DateTime>((dt, _) => dt.ToString("O"));

    return Verify(bericht)
      .ModifySerialization(_ => _.DontScrubDateTimes());
  }

  [Test]
  public void Soll_Reisen_außerhalb_der_EU_mit_100_EUR_pro_Tag_erstatten()
  {
    var anfang = DateTime.MinValue;
    var ende = anfang;
    var zielort = "egal";
    var grund = "egal";
    var formular = new Reisekostenformular(anfang, ende, zielort, grund);

    var buchhaltung = new Buchhaltung();
    buchhaltung.ErfasseReise(formular, this);

    var geo = A.Fake<ITranslateCitiesToEuCountries>();
    A.CallTo(() => geo.IsOutsideOfEu(A<string>.Ignored))
     .Returns(true);

    var bericht = Auswerten(buchhaltung, geo);

    bericht.Summe
           .Should()
           .Be(100);
  }

  static Bericht Auswerten(Buchhaltung buchhaltung, ITranslateCitiesToEuCountries geo)
    => buchhaltung.ErzeugeBericht(geo);
}
