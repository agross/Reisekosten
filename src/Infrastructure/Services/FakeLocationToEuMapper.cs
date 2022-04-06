using Domain.Services;

namespace Infrastructure.Services;

public class FakeLocationToEuMapper : ITranslateCitiesToEuCountries
{
  public bool IsOutsideOfEu(string city)
    => city != "Berlin";
}
