using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using MongoDB.Bson;

namespace DataAccess.Application
{
    public interface IRepository<T> where T : PersistentEntity
    {
        T GetById(ObjectId id);
        IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate = null);

        ObjectId Create(T @object);
        void Update(T @object);
        void Delete(ObjectId id);
    }
}