using Application.UseCases.Bericht;
using Application.UseCases.Erfassen;

using Domain.Entities;

using FluentAssertions;

using NUnit.Framework;

namespace Application.IntegrationTests.Behaviors;

[TestFixture]
public class ErfassenUndBerichtErzeugenBehavior : Behavior
{
  [Test]
  public async Task Soll_Formular_erfassen_und_Bericht_erzeugen()
  {
    await Reise.Erfassen(nach: "Leipzig", start: "heute 16:00", ende: "morgen 20:00");

    await Bericht.MitAnzahlEinträgen(1);
    await Bericht.MitSumme(18);
  }
}



















public class Behavior
{
  protected ReiseDsl Reise { get; }
  protected BerichtDsl Bericht { get; }

  protected Behavior()
  {
    Reise = new ReiseDsl();
    Bericht = new BerichtDsl();
  }
}

// Übersetzung der gut lesbaren DSL in die Domain oder API Calls oder
// UI-Interaktionen.
public class ReiseDsl
{
  public async Task Erfassen(string start, string ende, string nach = "egal", string grund = "egal")
  {
    await Testing.AddAsync(new Buchhaltung { Id = 42 });

    var command = new ErfassenCommand(ToDateTime(start),
                                      ToDateTime(ende),
                                      nach,
                                      grund);

    await Testing.SendAsync(command);
  }

  static DateTime ToDateTime(string str)
  {
    var parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    var date = parts[0] switch
    {
      "heute" => DateOnly.FromDateTime(DateTime.Today),
      "morgen" => DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                                        _ => throw new ArgumentOutOfRangeException()
    };

    var time = TimeOnly.Parse(parts[1]);

    return date.ToDateTime(time);
  }
}

public class BerichtDsl
{
  public async Task MitAnzahlEinträgen(int anzahl)
  {
    var query = new ErzeugeBerichtQuery();

    var bericht = await Testing.SendAsync(query);

    bericht.Should().HaveCount(anzahl);
  }

  public async Task MitSumme(decimal summe)
  {
    var query = new ErzeugeBerichtQuery();

    var bericht = await Testing.SendAsync(query);

    bericht.Summe.Should().Be(summe);
  }
}
