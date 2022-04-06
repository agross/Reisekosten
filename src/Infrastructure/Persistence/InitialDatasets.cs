using Domain.Entities;

namespace Infrastructure.Persistence;

public static class InitialDatasets
{
  public static readonly Buchhaltung[] Buchhaltung =
  {
    new() { Id = 42 },
  };
}
