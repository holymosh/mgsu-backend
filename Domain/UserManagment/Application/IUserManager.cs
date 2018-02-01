using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using Common.Entities;

namespace UserManagment.Application
{
    public interface IUserManager
    {
        User GetUserById(ObjectId id);
        IEnumerable<User> GetUserByPredicate(Expression<Func<User, bool>> predicate = null);

        ObjectId CreateUser(User userToCreate);
        void UpdateUser(User userToUpdate);
        void DeleteUser(ObjectId userId);
    }
}