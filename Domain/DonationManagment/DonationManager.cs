using System;
using System.Diagnostics;
using DataAccess.Application;
using DonationManagment.Application;
using Common.Entities;
using MongoDB.Bson;
using ProjectManagment.Application;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;

namespace DonationManagment
{
    public class DonationManager : IDonationManager
    {
        public Donation GetDonation(ObjectId donationId)
        {
            Require.NotNull(donationId, nameof(donationId));

            return _donationRepository.GetById(donationId);
        }

        public ObjectId CreateDonation(Donation donationToCreate)
        {
            Require.NotNull(donationToCreate, nameof(donationToCreate));

            var id = _donationRepository.Create(donationToCreate);

            

            if (donationToCreate.Confirmed)
            {
                Debug.WriteLine("Autoconfirmed donation creation");

                ConfirmDonation(donationToCreate);
            }

            return id;
        }

        public void ConfirmDonation(Donation donationToConfirm)
        {
            Require.NotNull(donationToConfirm, nameof(donationToConfirm));
            var targetProject = _projectManager.GetProjectById(donationToConfirm.ProjectId);

            Require.NotNull(targetProject, nameof(targetProject));
            _projectManager.GiveMoneyToProject(donationToConfirm.ProjectId, donationToConfirm.Value);

            donationToConfirm.Confirmed = true;
            _donationRepository.Update(donationToConfirm);
        }

        public void UpdateDonation(Donation donationToUpdate)
        {
            Require.NotNull(donationToUpdate, nameof(donationToUpdate));

            var oldDonation = _donationRepository.GetById(donationToUpdate.Id);

            if (oldDonation.Confirmed != donationToUpdate.Confirmed)
            {
                Debug.WriteLine("Implicit donation confirmation status changing");

                if (!donationToUpdate.Confirmed)
                {
                    throw new RollbackdonationException();
                }

               ConfirmDonation(donationToUpdate);

            }

            _donationRepository.Update(donationToUpdate);
        }

        public void DeleteDonation(ObjectId donationToDeleteId)
        {
            Require.NotNull(donationToDeleteId, nameof(donationToDeleteId));

            _donationRepository.Delete(donationToDeleteId);
        }

        public IEnumerable<Donation> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate = null)
        {
            return _donationRepository.GetByPredicate(predicate);
        }

        private readonly IRepository<Donation> _donationRepository;
        private readonly IProjectManager _projectManager;

        public DonationManager(IRepository<Donation> donationRepository, IProjectManager projectManager)
        {
            _donationRepository = donationRepository;
            _projectManager = projectManager;
        }
    }

    public class RollbackdonationException : Exception
    {
    }
}