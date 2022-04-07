using System.Threading.Tasks;

using Application;

using Domain.Entities;

namespace Web.ProviderTests.Persistence;

class InMemoryBuchhaltungRepository : IBuchhaltungRepository
{
  Buchhaltung _buchhaltung;

  public Task<Buchhaltung> LoadBuchhaltung() => Task.FromResult(_buchhaltung);

  public Task SaveChangesAsync() => Task.CompletedTask;

  public void SetState(Buchhaltung buchhaltung)
  {
    _buchhaltung = buchhaltung;
  }
}
