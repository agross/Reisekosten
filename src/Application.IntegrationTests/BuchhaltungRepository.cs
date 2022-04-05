using Domain.Entities;

using Marten;

namespace Application.IntegrationTests;

public class BuchhaltungRepository : IBuchhaltungRepository
{
  int _id;
  readonly IDocumentSession _session;

  public BuchhaltungRepository(IDocumentStore store)
  {
    _session = store.DirtyTrackedSession();
  }

  internal void SetBuchhaltungIdForTesting(int id)
  {
    _id = id;
  }

  public Task<Buchhaltung?> LoadBuchhaltung()
    => _session.LoadAsync<Buchhaltung>(_id);

  public Task SaveChangesAsync()
    => _session.SaveChangesAsync();
}
