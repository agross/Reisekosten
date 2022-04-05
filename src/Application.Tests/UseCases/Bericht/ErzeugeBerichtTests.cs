using Application.UseCases.Bericht;

using Domain.Services;

using FakeItEasy;

using FluentAssertions;

using NUnit.Framework;

namespace Application.Tests.UseCases.Bericht;

[TestFixture]
public class ErzeugeBerichtTests
{
  [Test]
  public async Task Soll_leeren_Bericht_erzeugen()
  {
    var query = new ErzeugeBerichtQuery();

    var handler = new ErzeugeBerichtQueryHandler(A.Fake<IBuchhaltungRepository>(),
                                                 A.Fake<ITranslateCitiesToEuCountries>());

    var bericht = await handler.Handle(query, CancellationToken.None);

    bericht.Summe.Should().Be(0);
  }
}
