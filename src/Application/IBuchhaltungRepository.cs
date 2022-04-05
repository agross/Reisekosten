using Domain.Entities;

namespace Application;

public interface IBuchhaltungRepository
{
  Task<Buchhaltung> LoadBuchhaltung();
  Task SaveChangesAsync();
}
