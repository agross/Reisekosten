using Domain.Entities;

using Marten;

namespace Application.IntegrationTests;

public class BuchhaltungRepository : IBuchhaltungRepository
{
  readonly IDocumentSession _session;

  public BuchhaltungRepository(IDocumentStore store)
  {
    _session = store.DirtyTrackedSession();
  }

  public Task<Buchhaltung?> LoadBuchhaltung()
    => _session.LoadAsync<Buchhaltung>(42);

  public Task SaveChangesAsync()
    => _session.SaveChangesAsync();
}
