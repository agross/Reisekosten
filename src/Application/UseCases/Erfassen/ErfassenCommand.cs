using Domain.Entities;
using Domain.Services;

using MediatR;

namespace Application.UseCases.Erfassen;

public record ErfassenCommand(DateTime Anfang,
                              DateTime Ende,
                              string Zielort,
                              string Grund) : IRequest;

public class ErfassenCommandHandler : IRequestHandler<ErfassenCommand>
{
  readonly ISystemClock _clock;
  readonly IBuchhaltungRepository _db;

  public ErfassenCommandHandler(ISystemClock clock, IBuchhaltungRepository db)
  {
    _clock = clock;
    _db = db;
  }

  public async Task<Unit> Handle(ErfassenCommand request, CancellationToken cancellationToken)
  {
    var buchhaltung = await _db.LoadBuchhaltung();

    var formular = new Reisekostenformular(request.Anfang,
                                           request.Ende,
                                           request.Zielort,
                                           request.Grund);

    buchhaltung.ErfasseReise(formular, _clock);

    await _db.SaveChangesAsync();

    return Unit.Value;
  }
}
