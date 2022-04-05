namespace Domain.Services;

public interface ITranslateCitiesToEuCountries
{
  bool IsOutsideOfEu(string city) => false;
}