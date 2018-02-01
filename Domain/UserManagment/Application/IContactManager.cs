using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using Common.Entities;

namespace UserManagment.Application
{
    public interface IContactManager
    {
        Contact GetContactById(ObjectId contactid);
        IEnumerable<Contact> GetContactByPredicate(Expression<Func<Contact, bool>> predicate = null);

        ObjectId CreateContact(Contact contactToCreate);
        void UpdateContact(Contact contactToUpdate);
        void DeleteContact(ObjectId contactId);
    }
}