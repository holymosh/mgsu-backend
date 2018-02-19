using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UserManagment.Application;
using Common.Entities;
using MGSUCore.Authentification;
using Microsoft.AspNetCore.Authorization;
using MGSUCore.Filters;
using MGSUCore.Models;
using Newtonsoft.Json;
using MGSUCore.Models.Convertors;
using MGSUCore.Models.Mappers;

namespace MGSUCore.Controllers
{
    [CustomExceptionFilterAttribute]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            return _userManager.GetUserByPredicate()
                .Select(UserMapper.UserToUserModel);
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var user = _userManager.GetUserById(objectId);
            if (user == null)
                return NotFound();

            return Ok(UserMapper.UserToUserModel(user));
        }

        // POST: Users
        [HttpPost]
        public IActionResult Post([FromBody] UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!Password.IsStringCorrectPassword(userModel.Password))
                return BadRequest("Password is not satisfy security requirements");

            var userToCreate = new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                MiddleName = userModel.MiddleName,
                CreationTime = new BsonDateTime(DateTime.UtcNow),
                Email = userModel.Email,
                IsConfirmed = false,
                Phone = userModel.Phone,
                Password = new Password(userModel.Password),
                Role = UserRole.User,
                UserProfile = userModel.UserProfile
            };

            var id = _userManager.CreateUser(userToCreate);

            return Ok(id);
        }

        // PUT: Users/5
        [Authorize("User")]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var userId = User.GetId();

            if (userId != objectId)
                return Unauthorized();
            
            if (_userManager.GetUserById(objectId) == null)
                return NotFound();

            userModel.Id = userId;

            var userToUpdateNew = UserMapper.UserModelToUser(userModel);
            _userManager.UpdateUser(userToUpdateNew);

            return Ok(id);
        }

        // DELETE: Users/5
        [Authorize("User")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            if (User.GetId() != objectId)
                return Unauthorized();
                
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_userManager.GetUserById(objectId) == null)
                return NotFound();

            _userManager.DeleteUser(objectId);

            return Ok(id);
        }
        
    }
}