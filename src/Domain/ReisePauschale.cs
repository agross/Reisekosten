namespace Domain;

public record ReisePauschale(DateTime Anfang,
                             DateTime Ende,
                             string Zielort,
                             string Grund,
                             decimal Pauschale);
