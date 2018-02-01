using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using DataAccess.Application;
using MongoDB.Bson;
using UserManagment.Application;
using Common.Entities;

namespace UserManagment
{
    public class ContactManager : IContactManager
    {
        private readonly IRepository<Contact> _contactRepository;

        public ContactManager(IRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Contact GetContactById(ObjectId contactid)
        {
            Require.NotNull(contactid, nameof(contactid));

            return _contactRepository.GetById(contactid);
        }

        public IEnumerable<Contact> GetContactByPredicate(Expression<Func<Contact, bool>> predicate = null)
        {
            return _contactRepository.GetByPredicate(predicate);
        }

        public ObjectId CreateContact(Contact contactToCreate)
        {
            Require.NotNull(contactToCreate, nameof(contactToCreate));

            return _contactRepository.Create(contactToCreate);
        }

        public void UpdateContact(Contact contactToUpdate)
        {
            Require.NotNull(contactToUpdate, nameof(contactToUpdate));

            _contactRepository.Update(contactToUpdate);
        }

        public void DeleteContact(ObjectId contactId)
        {
            Require.NotNull(contactId, nameof(contactId));

            _contactRepository.Delete(contactId);
        }
    }
}