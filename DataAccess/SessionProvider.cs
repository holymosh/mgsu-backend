using Common;
using DataAccess.Application;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace DataAccess
{
    public class SessionProvider : ISessionProvider
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public SessionProvider(string connectionString , IEntityMapper mapper)
        {
            Require.NotEmpty(connectionString, nameof(connectionString));

            var connectionStringValidated = new ConnectionString(connectionString);

            _client = new MongoClient(connectionStringValidated.ToString());
            _database = _client.GetDatabase(connectionStringValidated.DatabaseName);
            mapper.CreateMappings(_database);

        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}