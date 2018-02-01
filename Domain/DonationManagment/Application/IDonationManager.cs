using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Entities;
using MongoDB.Bson;

namespace DonationManagment.Application
{
    public interface IDonationManager
    {
        IEnumerable<Donation> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate = null);
        Donation GetDonation(ObjectId donationId);
        ObjectId CreateDonation(Donation donationToCreate);
        void ConfirmDonation(Donation donationToConfirm);

        void UpdateDonation(Donation donationToUpdate);
        void DeleteDonation(ObjectId donationToDeleteId);
    }
}