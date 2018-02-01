using MongoDB.Driver;

namespace DataAccess.Application
{
    public interface ISessionProvider
    {
        IMongoCollection<T> GetCollection<T>();
    }
}