namespace Domain;

public class Bericht
{
  readonly List<ReisePauschale> _pauschalen = new();

  public Bericht(IEnumerable<ReisePauschale> pauschalen)
  {
    _pauschalen.AddRange(pauschalen);
  }

  public decimal Summe
  {
    get
    {
      return _pauschalen.Sum(p => p.Pauschale);
    }
  }
}
