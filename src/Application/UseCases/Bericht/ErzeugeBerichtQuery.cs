using Domain.Services;

using MediatR;

namespace Application.UseCases.Bericht;

public class ErzeugeBerichtQuery : IRequest<BerichtViewModel>
{
}

public class ErzeugeBerichtQueryHandler : IRequestHandler<ErzeugeBerichtQuery, BerichtViewModel>
{
  readonly IBuchhaltungRepository _db;
  readonly ITranslateCitiesToEuCountries _geo;

  public ErzeugeBerichtQueryHandler(IBuchhaltungRepository db, ITranslateCitiesToEuCountries geo)
  {
    _db = db;
    _geo = geo;
  }

  public async Task<BerichtViewModel> Handle(ErzeugeBerichtQuery request,
                                             CancellationToken cancellationToken)
  {
    var buchhaltung = await _db.LoadBuchhaltung();

    var bericht = buchhaltung.ErzeugeBericht(_geo);

    return new BerichtViewModel
    {
      Summe = bericht.Summe,
      Reisen = bericht.Select(r => new ReiseViewModel
      {
        Anfang = r.Anfang.ToLocalTime(),
        Ende = r.Ende.ToLocalTime(),
        Grund = r.Grund,
        Zielort = r.Zielort,
        Pauschale = r.Pauschale,
      }),
    };
  }
}
