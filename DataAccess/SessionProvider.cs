using System;
using System.Linq;
using Common;
using Common.Entities;
using DataAccess.Application;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace DataAccess
{
    public class SessionProvider : ISessionProvider
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private bool _isFundProjectCreated;
        private bool _isHundredYearsProjectCreated;

        public SessionProvider(string connectionString )
        {
            Require.NotEmpty(connectionString, nameof(connectionString));

            var connectionStringValidated = new ConnectionString(connectionString);

            _client = new MongoClient(connectionStringValidated.ToString());
            _database = _client.GetDatabase(connectionStringValidated.DatabaseName);
            if (!(_isHundredYearsProjectCreated && _isFundProjectCreated))
            {
                CheckFundAndHundredYearsProjectsExistence();
            }
        }

        private void CheckFundAndHundredYearsProjectsExistence()
        {
            var collectionRepository = new Repository<Project>(this);
            var fundProject = collectionRepository.GetByPredicate(project => project.Direction.Equals("fund"));
            if (fundProject.Count().Equals(0))
            {
                collectionRepository.Create(new Project()
                {
                    Direction = "fund",
                    Content = "Эндаумент фонд МГСУ",
                    CreatingDate = DateTime.Now,
                    Given = Decimal.Zero,
                    IsDeleted = false,
                    Need = 10000000,
                    Name = "Эндаумент фонд МГСУ",
                    Public = false,
                    ShortDescription = "проект эндаумент фонда МГСУ"
                });
                _isFundProjectCreated = true;
            }
            var hundredYearsProject =
                collectionRepository.GetByPredicate(project => project.Direction.Equals("hundred"));
            if (hundredYearsProject.Count().Equals(0))
            {
                collectionRepository.Create(new Project()
                {
                    Direction = "hundred",
                    Content = "Юбилей МГСУ",
                    CreatingDate = DateTime.Now,
                    Given = Decimal.Zero,
                    IsDeleted = false,
                    Need = 10000000,
                    Name = "100-летие",
                    Public = false,
                    ShortDescription = "проект эндаумент фонда МГСУ"
                });
                _isHundredYearsProjectCreated = true;
            }
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}