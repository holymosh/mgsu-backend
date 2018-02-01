using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using DataAccess.Application;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : PersistentEntity
    {
        private readonly IMongoCollection<T> _collection;

        public Repository(ISessionProvider sessionProvider)
        {
            _collection = sessionProvider.GetCollection<T>();
        }

        public T GetById(ObjectId id)
        {
            Require.NotNull(id, nameof(id));

            var eq = Builders<T>.Filter.And(Builders<T>.Filter.Eq("Id", id), Builders<T>.Filter.Eq("IsDeleted", false));

            return _collection.Find(eq).SingleOrDefault();
        }

        public IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate = null)
        {
            var undeletedItems = _collection.AsQueryable().Where(_ => !_.IsDeleted);
            return predicate == null
                ? undeletedItems
                : undeletedItems.Where(predicate);
        }

        public ObjectId Create(T @object)
        {
            Require.NotNull(@object, nameof(@object));

            @object.CreatingDate = DateTime.Now;

            _collection.InsertOne(@object);

            return @object.Id;
        }

        public void Update(T @object)
        {
            Require.NotNull(@object, nameof(@object));

            var eq = Builders<T>.Filter.And(Builders<T>.Filter.Eq("Id", @object.Id),
                Builders<T>.Filter.Eq("IsDeleted", false));

            _collection.ReplaceOne(eq, @object);
        }

        public void Delete(ObjectId id)
        {
            Require.NotNull(id, nameof(id));

            var objToDelete = GetById(id);

            if (objToDelete == null) return;

            objToDelete.IsDeleted = true;

            Update(objToDelete);
        }
    }
}