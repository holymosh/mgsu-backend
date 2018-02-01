using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Entities;
using MongoDB.Bson;
using ProjectManagment.Application;
using DataAccess.Application;
using System.Linq;
using Common;

namespace ProjectManagment
{
    public class ProjectManager : IProjectManager
    {
        private readonly IRepository<Project> _projectRepository;

        public ProjectManager(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public ObjectId CreateProject(Project projectToCreate)
        {
            Require.NotNull(projectToCreate, nameof(projectToCreate));

            return _projectRepository.Create(projectToCreate);
        }

        public void DeleteProject(ObjectId projectIdToDelete)
        {
			Require.NotNull(projectIdToDelete, nameof(projectIdToDelete));

            _projectRepository.Delete(projectIdToDelete);
        }

        public Project FindFundProject()
        {
            return _projectRepository.GetByPredicate(project => project.Direction == "fund").Single();
        }

        public Project GetProjectById(ObjectId projectId)
        {
            Require.NotNull(projectId, nameof(projectId));

            return _projectRepository.GetById(projectId);
        }

        public IEnumerable<Project> GetProjectByPredicate(Expression<Func<Project, bool>> predicate = null)
        {
            return _projectRepository.GetByPredicate(predicate);
        }

        public void GiveMoneyToProject(ObjectId projectId, decimal sum)
        {
            Require.NotNull(projectId, nameof(projectId));
            Require.Positive(sum, nameof(sum));

            var fund = FindFundProject();
            var aimedProject = GetProjectById(projectId);

            aimedProject.Given += sum;

			_projectRepository.Update(aimedProject);

            //Если пожертвовали не на фонд целиком, увеличить суммарное количество средств фонда
            if(!projectId.Equals(fund.Id))
            {
                GiveMoneyToProject(fund.Id, sum);
            }
        }

        public void UpdateProject(Project projectToUpdate)
        {
            Require.NotNull(projectToUpdate, nameof(projectToUpdate));

            _projectRepository.Update(projectToUpdate);
        }
    }
}
