namespace Catalog.API.Repositories
{
    public interface IAuditRepository
    {
        Task LogAsync(Guid plateId, string action);
    }
}
