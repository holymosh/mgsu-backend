using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using Common.Entities;
using DataAccess.Application;
using MongoDB.Bson;

namespace PartnershipManagment
{
    public class PartnershsipManager:IPartnershipManager
    {
        private readonly IRepository<Partnership> _partnershipRepository;

        public PartnershsipManager(IRepository<Partnership> repository)
        {
            _partnershipRepository = repository;
        }

        public ObjectId CreatePartnership(Partnership partnershipToCreate)
        {
            Require.NotNull(partnershipToCreate,nameof(partnershipToCreate));
            return _partnershipRepository.Create(partnershipToCreate);
        }

        public void DeletePartnership(ObjectId partnershipToDelete)
        {
            Require.NotNull(partnershipToDelete,nameof(partnershipToDelete));
            _partnershipRepository.Delete(partnershipToDelete);
        }

        public Partnership GetById(ObjectId id)
        {
            Require.NotNull(id,nameof(id));
            return _partnershipRepository.GetById(id);
        }

        public IEnumerable<Partnership> GetByPredicate(Expression<Func<Partnership, bool>> predicate = null)
        {
            return _partnershipRepository.GetByPredicate(predicate);
        }

        public void UpdatePartnership(Partnership partnership)
        {
            Require.NotNull(partnership,nameof(partnership));
            _partnershipRepository.Update(partnership);
        }
    }
}
