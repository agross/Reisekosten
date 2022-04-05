using Domain.Services;

using MediatR;

namespace Application.UseCases.Bericht;

public class ErzeugeBerichtQuery : IRequest<Domain.Entities.Bericht>
{
}

public class ErzeugeBerichtQueryHandler : IRequestHandler<ErzeugeBerichtQuery, Domain.Entities.Bericht>
{
  readonly IBuchhaltungRepository _db;
  readonly ITranslateCitiesToEuCountries _geo;

  public ErzeugeBerichtQueryHandler(IBuchhaltungRepository db, ITranslateCitiesToEuCountries geo)
  {
    _db = db;
    _geo = geo;
  }

  public async Task<Domain.Entities.Bericht> Handle(ErzeugeBerichtQuery request, CancellationToken cancellationToken)
  {
    var buchhaltung = await _db.LoadBuchhaltung();

    return buchhaltung.ErzeugeBericht(_geo);
  }
}
