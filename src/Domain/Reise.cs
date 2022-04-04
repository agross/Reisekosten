namespace Domain;

public record Reise(Reisekostenformular Formular)
{
  public decimal Pauschale { get
  {
    if (Dauer >= TimeSpan.FromHours(24))
    {
      return 24;
    }

    if (Dauer >= TimeSpan.FromHours(12))
    {
      return 12;
    }

    if (Dauer >= TimeSpan.FromHours(8))
    {
      return 6;
    }

    return 0;
  }}

  TimeSpan Dauer { get
  {
    return Formular.Ende - Formular.Anfang;
  }}
}
