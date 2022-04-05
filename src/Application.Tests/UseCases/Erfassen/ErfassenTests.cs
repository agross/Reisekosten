using Application.UseCases.Erfassen;

using Domain.Services;

using FakeItEasy;

using NUnit.Framework;

namespace Application.Tests.UseCases.Erfassen;

[TestFixture]
public class ErfassenTests
{
  [Test]
  public void Soll_Formular_erfassen()
  {
    var command = new ErfassenCommand(DateTime.MinValue,
                                      DateTime.MinValue,
                                      "zielort",
                                      "grund");

    var handler = new ErfassenCommandHandler(A.Fake<ISystemClock>(),
                                             A.Fake<IBuchhaltungRepository>());

    handler.Handle(command, CancellationToken.None);
  }
}
