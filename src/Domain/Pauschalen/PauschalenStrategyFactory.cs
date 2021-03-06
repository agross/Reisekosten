using Domain.Services;

namespace Domain.Pauschalen;

class PauschalenStrategyFactory
{
  public IPauschalenStrategy Resolve(ITranslateCitiesToEuCountries geo,
                                     string reiseziel)
  {
    if (geo.IsOutsideOfEu(reiseziel))
    {
      return new PauschaleAu├čerhalbDerEu();
    }

    return new PauschaleInnerhalbDerEu();
  }
}
