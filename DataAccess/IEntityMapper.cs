using MongoDB.Driver;

namespace DataAccess
{
    public interface IEntityMapper
    {
        void CreateMappings(IMongoDatabase database);
    }
}