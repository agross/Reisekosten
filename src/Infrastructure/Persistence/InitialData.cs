using Marten;
using Marten.Schema;

namespace Infrastructure.Persistence;

public class InitialData : IInitialData
{
  readonly object[] _initialData;

  public InitialData(params object[] initialData)
  {
    _initialData = initialData;
  }

  public async Task Populate(IDocumentStore store, CancellationToken cancellation)
  {
    await using var session = store.LightweightSession();

    // Marten UPSERT will cater for existing records.
    session.Store(_initialData);

    await session.SaveChangesAsync(cancellation);
  }
}
