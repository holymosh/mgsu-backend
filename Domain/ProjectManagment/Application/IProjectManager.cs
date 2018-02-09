using MongoDB.Bson;
using Common.Entities;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ProjectManagment.Application
{
    public interface IProjectManager
    {
        Project GetProjectById(ObjectId projectId);
        IEnumerable<Project> GetProjectByPredicate(Expression<Func<Project, bool>> projectId = null);

		ObjectId CreateProject(Project projectToCreate);
        void GiveMoneyToProject(ObjectId projectId, decimal sum);

        void UpdateProject(Project projectToUpdate);
        void DeleteProject(ObjectId projectIdToDelete);

    }
}