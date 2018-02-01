using System.Threading.Tasks;
using DataAccess.Application;
using MongoDB.Driver;
using Common.Entities;
using System;
using System.Linq.Expressions;

namespace DataAccess
{
    public static class DataConstraintsProvider
    {
        
        public static async Task CreateConstraints(ISessionProvider sessionProvider)
        {
            //User constraints
            var users = sessionProvider.GetCollection<User>();
            await users.Indexes.DropAllAsync();
            await users.AddUniqueIndex(_ => _.Email);
            
            //Project constraints
            var projects = sessionProvider.GetCollection<Project>();
            await projects.Indexes.DropAllAsync();
            await projects.AddUniqueIndex(_ => _.Name);
        }
        
        private static async Task AddUniqueIndex<T>(this IMongoCollection<T> collection, Expression<Func<T, object>> idxName)
        {
            await collection.Indexes.CreateOneAsync(
                Builders<T>.IndexKeys.Ascending(idxName),
                new CreateIndexOptions
                {
                    Unique = true
                });
        }
        
    }
}