using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using MGSUCore.Models;
using MGSUCore.Models.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectManagment.Application;

namespace MGSUCore.Controllers
{
    [Route("[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IProjectManager _projectManager;

        public ProjectsController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            IEnumerable<Project> projectsToReturn;
            if (!Request.Query.TryGetValue("direction", out Microsoft.Extensions.Primitives.StringValues value))
            {
                if (User.IsInRole("Admin"))
                {
                    projectsToReturn = _projectManager.GetProjectByPredicate();
                    return Ok(projectsToReturn.Select(ProjectMapper.ProjectToProjectModel));
                }
                projectsToReturn = _projectManager.GetProjectByPredicate(project => project.Public);
                return Ok(projectsToReturn.Select(ProjectMapper.ProjectToProjectModel));
            }

            if (value.Count > 1)
            {
                return BadRequest("Query bad");
            }

            var direction = value.Single();

            projectsToReturn = _projectManager.GetProjectByPredicate(project => project.Direction == direction);
            return Ok(projectsToReturn.Select(ProjectMapper.ProjectToProjectModel));
        }

        [HttpGet]
        [Route("hundred")]
        public IActionResult GetHundredYearsProject()
        {
            return Ok(_projectManager.GetProjectByPredicate(project => project.Direction.Equals("hundred")).First());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var projectToReturn = _projectManager.GetProjectById(objectId);

            if (projectToReturn == null)
                return NotFound();

            return Ok(ProjectMapper.ProjectToProjectModel(projectToReturn));
        }

        // POST api/values
        [HttpPost]
        [Authorize("Admin")]
        //[Route("create")]
        public IActionResult CreateProject([FromBody] ProjectModel projectModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postToCreate = ProjectMapper.ProjectModelToProject(projectModel);

            return Ok(_projectManager.CreateProject(postToCreate).ToString());
        }

        [HttpGet]
        [Route("example")]
        public IActionResult GetProjectModelExample()
        {
            var result = new ProjectModel()
            {
                Name = "mgsu",
                CreatingDate = DateTime.Now,
                Content = "testcontent",
                Direction = "testDirection",
                Given = Decimal.One,
                Img = new ImageModel() { Role = "role",Original = "original",Small = "small"},
                Id = ObjectId.GenerateNewId(),
                Need = Decimal.One,
                ShortDescription = "short description",
                Public = true
            };
            return Ok(result);
        }

        // PUT values/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult UpdateProject(string id, [FromBody] ProjectModel projectModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldProject = _projectManager.GetProjectById(objectId);

            if (oldProject == null)
                return NotFound();

            oldProject.Name = projectModel.Name ?? oldProject.Name;
            oldProject.Direction = projectModel.Direction ?? oldProject.Direction;
            oldProject.ShortDescription = projectModel.ShortDescription ?? oldProject.ShortDescription;
            oldProject.Content = projectModel.Content ?? oldProject.Content;
            oldProject.Img = projectModel.Img != null
                ? new Image
                {
                    Original = projectModel.Img.Original,
                    Small = projectModel.Img.Small,
                    Role = projectModel.Img.Role
                }
                : oldProject.Img;
            oldProject.Need = projectModel.Need == 0 ? oldProject.Need : projectModel.Need;
            oldProject.Given = projectModel.Given == 0 ? oldProject.Given : projectModel.Given;
            oldProject.Public = projectModel.Public;

            _projectManager.UpdateProject(oldProject);
            return Ok(id);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult DeleteProject(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldProject = _projectManager.GetProjectById(objectId);

            if (oldProject == null)
                return NotFound();

            _projectManager.DeleteProject(objectId);
            return Ok(id);
        }
    }
}