namespace Domain;

public interface ITranslateCitiesToEuCountries
{
  bool IsOutsideOfEu(string city) => false;
}