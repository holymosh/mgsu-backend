using Common;
using Common.Entities;
using DataAccess.Application;
using MongoDB.Bson;
namespace PartnershipManagment
{
    public class PartnershipManager : IPartnershopManager
    {
        private readonly IRepository<Partnership> _partnershipRepository;

        public PartnershipManager(IRepository<Partnership> partnershipRepository)
        {
            _partnershipRepository = partnershipRepository;
        }

        public ObjectId CreatePartnership(Partnership partnershipToCreate)
        {
            Require.NotNull(partnershipToCreate, nameof(partnershipToCreate));
            return _partnershipRepository.Create(partnershipToCreate);
        }
    }
}