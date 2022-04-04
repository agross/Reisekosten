namespace Domain;

public record Reisekostenformular
{
  public Reisekostenformular(DateTime Anfang,
                             DateTime Ende,
                             string Zielort,
                             string Grund)
  {
    if (Ende < Anfang)
    {
      throw new EndeDerReiseMussNachReisebeginnLiegen();
    }

    this.Anfang = Anfang;
    this.Ende = Ende;
    this.Zielort = Zielort;
    this.Grund = Grund;
  }

  public DateTime Anfang { get; init; }
  public DateTime Ende { get; init; }
  public string Zielort { get; init; }
  public string Grund { get; init; }

  public void Deconstruct(out DateTime Anfang,
                          out DateTime Ende,
                          out string Zielort,
                          out string Grund)
  {
    Anfang = this.Anfang;
    Ende = this.Ende;
    Zielort = this.Zielort;
    Grund = this.Grund;
  }
}
