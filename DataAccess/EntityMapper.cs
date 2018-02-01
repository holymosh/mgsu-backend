using System;
using Common.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DataAccess
{
    public class EntityMapper : IEntityMapper
    {
        public void CreateMappings(IMongoDatabase database)
        {
            BsonClassMap.RegisterClassMap<Attendance>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<Contact>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<Event>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<Image>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<Post>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<Project>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<User>(mapper => mapper.AutoMap());
            BsonClassMap.RegisterClassMap<UserProfile>(mapper => mapper.AutoMap());
            try
            {
                database.CreateCollection(typeof(Attendance).Name);
                database.CreateCollection(typeof(Contact).Name);
                database.CreateCollection(typeof(Event).Name);
                database.CreateCollection(typeof(Image).Name);
                database.CreateCollection(typeof(Post).Name);
                database.CreateCollection(typeof(Project).Name);
                database.CreateCollection(typeof(User).Name);
                database.CreateCollection(typeof(UserProfile).Name);
                database.CreateCollection(typeof(Donation).Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}