namespace Application.UseCases.Bericht;

public class BerichtViewModel
{
  public decimal Summe { get; set; }
  public IEnumerable<ReiseViewModel> Reisen { get; set; } = new List<ReiseViewModel>();
}

public class ReiseViewModel
{
  public DateTime Anfang { get; set; }
  public DateTime Ende { get; set; }
  public string Grund { get; set; }
  public string Zielort { get; set; }
  public decimal Pauschale { get; set; }
}
