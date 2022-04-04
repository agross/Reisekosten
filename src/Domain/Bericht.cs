using System.Collections;

namespace Domain;

public class Bericht : IEnumerable<ReisePauschale>
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

  public IEnumerator<ReisePauschale> GetEnumerator()
    => _pauschalen.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator()
    => ((IEnumerable) _pauschalen).GetEnumerator();
}
