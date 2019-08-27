namespace Marketplace.Framework
{
    using System.Threading.Tasks;

    public interface IEntityStore
    {
        Task<T> Load<T>(string entityId) where T : Entity;
        Task Save<T>(T entity) where T : Entity;
        Task<bool> Exists<T>(string entityId);
    }
}