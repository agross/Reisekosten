namespace Domain;

public record Reisekostenformular(DateTime Anfang,
                                  DateTime Ende,
                                  string Zielort,
                                  string Grund);
