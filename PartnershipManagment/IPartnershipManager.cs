using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Entities;
using MongoDB.Bson;

namespace PartnershipManagment
{
    public interface IPartnershipManager
    {
        ObjectId CreatePartnership(Partnership partnershipToCreate);
        void DeletePartnership(ObjectId partnershipToDelete);
        Partnership GetById(ObjectId id);
        IEnumerable<Partnership> GetByPredicate(Expression<Func<Partnership, bool>> predicate = null);
        void UpdatePartnership(Partnership partnership);
    }
}