using Application.UseCases.Erfassen;

using Domain.Entities;
using Domain.Services;

using FakeItEasy;

using FluentAssertions;

using NUnit.Framework;

namespace Application.IntegrationTests.Behaviors;

[TestFixture]
public class ErfassenUndBerichtErzeugenTests
{
  [Test]
  public async Task Soll_Formular_erfassen_und_Bericht_erzeugen()
  {
    await Testing.AddAsync(new Buchhaltung { Id = 42 });

    var command = new ErfassenCommand(DateTime.MinValue,
                                      DateTime.MinValue,
                                      "zielort",
                                      "grund");

    await Testing.SendAsync(command);

    var buchhaltung = await Testing.LoadAsync<Buchhaltung>(42);

    var bericht = buchhaltung.ErzeugeBericht(A.Fake<ITranslateCitiesToEuCountries>());

    bericht.Should().HaveCount(1);
  }
}
