namespace Domain.Pauschalen;

class PauschalenStrategyFactory
{
  public IPauschalenStrategy Resolve(ITranslateCitiesToEuCountries geo,
                                     string reiseziel)
  {
    if (geo.IsOutsideOfEu(reiseziel))
    {
      return new PauschaleAußerhalbDerEu();
    }

    return new PauschaleInnerhalbDerEu();
  }
}
