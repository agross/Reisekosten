using Domain.Entities;
using Domain.Errors;

using FluentAssertions;

namespace Domain.Tests;

[TestFixture]
public class ReisekostenformularTests
{
  [Test]
  public void Soll_Ende_vor_Anfang_ablehnen()
  {
    FluentActions.Invoking(() => new Reisekostenformular(DateTime.MaxValue,
                                                         DateTime.MinValue, "egal", "egal"))
                 .Should()
                 .Throw<EndeDerReiseMussNachReisebeginnLiegen>();
  }
}
