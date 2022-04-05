using Domain.Services;

namespace Application.IntegrationTests;

public class AlwaysInsideEu : ITranslateCitiesToEuCountries
{
  public bool IsOutsideOfEu(string city) => false;
}
