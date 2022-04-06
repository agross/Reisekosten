using System.Globalization;

using Application.UseCases.Bericht;
using Application.UseCases.Erfassen;

using Domain.Entities;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace Application.IntegrationTests.Behaviors;

[TestFixture]
public class ErfassenUndBerichtErzeugenBehavior : Behavior
{
  [Test]
  public async Task Soll_Formular_erfassen_und_Bericht_erzeugen()
  {
    await Buchhaltung.Existiert();

    await Reise.Erfassen(nach: "Leipzig", start: "heute 16:00", ende: "morgen 20:00");

    await Bericht.MitAnzahlEinträgen(1);
    await Bericht.MitSumme(18);
  }

  [Test]
  public async Task Soll_mehrere_Reisen_erfassen_und_Bericht_erzeugen()
  {
    await Buchhaltung.Existiert();

    await Reise.Erfassen(nach: "Leipzig", start: "heute 16:00", ende: "morgen 20:00");
    await Reise.Erfassen(nach: "Hamburg", start: "01.02.2022 16:00", ende: "02.02.2022 20:00");

    await Bericht.MitAnzahlEinträgen(2);
    await Bericht.MitSumme(36);
  }
}

public class Behavior
{
  public IServiceScope Scope { get; private set; } = null!;
  protected BuchhaltungDsl Buchhaltung { get; private set; } = null!;
  protected ReiseDsl Reise { get; private set; } = null!;
  protected BerichtDsl Bericht { get; private set; } = null!;
  public int BuchhaltungId { get; set; }

  [SetUp]
  public void BeforeEach()
  {
    Scope = Testing.ScopeFactory.CreateScope();

    Buchhaltung = new BuchhaltungDsl(this, TestContext.CurrentContext);
    Reise = new ReiseDsl(this);
    Bericht = new BerichtDsl(this);
  }

  [TearDown]
  public void AfterEach()
  {
    Scope.Dispose();
  }
}

// Übersetzung der gut lesbaren DSL in die Domain oder API Calls oder
// UI-Interaktionen.
public class BuchhaltungDsl
{
  readonly Behavior _behavior;
  readonly TestContext _testContext;

  public BuchhaltungDsl(Behavior behavior, TestContext testContext)
  {
    _behavior = behavior;
    _testContext = testContext;
  }

  public async Task Existiert()
  {
    _behavior.BuchhaltungId = _testContext.Random.Next();

    Testing.SetBuchhaltungIdForTesting(_behavior.BuchhaltungId, _behavior.Scope);

    await Testing.AddAsync(new Buchhaltung { Id = _behavior.BuchhaltungId });
  }
}

public class ReiseDsl
{
  readonly Behavior _behavior;

  public ReiseDsl(Behavior behavior)
  {
    _behavior = behavior;
  }

  public async Task Erfassen(string start, string ende, string nach = "egal", string grund = "egal")
  {
    var command = new ErfassenCommand(ToDateTime(start),
                                      ToDateTime(ende),
                                      nach,
                                      grund);

    await Testing.SendAsync(command, _behavior.Scope);
  }

  static DateTime ToDateTime(string str)
  {
    if (DateTime.TryParse(str,
                          CultureInfo.GetCultureInfo("de"),
                          DateTimeStyles.AssumeLocal,
                          out var result))
    {
      return result;
    }

    var parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    var date = parts[0] switch
    {
      "heute" => DateOnly.FromDateTime(DateTime.Today),
      "morgen" => DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
      _ => throw new ArgumentOutOfRangeException(),
    };

    var time = TimeOnly.Parse(parts[1]);

    return date.ToDateTime(time);
  }
}

public class BerichtDsl
{
  readonly Behavior _behavior;

  public BerichtDsl(Behavior behavior)
  {
    _behavior = behavior;
  }

  public async Task MitAnzahlEinträgen(int anzahl)
  {
    var query = new ErzeugeBerichtQuery();

    var bericht = await Testing.SendAsync(query, _behavior.Scope);

    bericht.Reisen.Should().HaveCount(anzahl);
  }

  public async Task MitSumme(decimal summe)
  {
    var query = new ErzeugeBerichtQuery();

    var bericht = await Testing.SendAsync(query, _behavior.Scope);

    bericht.Summe.Should().Be(summe);
  }
}
