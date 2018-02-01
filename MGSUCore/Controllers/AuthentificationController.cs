using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using UserManagment.Application;
using Common.Entities;
using MGSUCore.Authentification;
using MGSUCore.Filters;
using MGSUCore.Models;
using MGSUCore.Models.Mappers;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson;

namespace MGSUCore.Controllers
{
    [CustomExceptionFilterAttribute]
    public class AuthentificationController : Controller
    {
        private readonly IUserManager _userManager;

        public AuthentificationController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("user/login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            var intentedUser = _userManager.GetUserByPredicate(user => user.Email == credentials.Email).SingleOrDefault();
            if(intentedUser == null)
            {
                return NotFound();
            }

            var intendedHash = new Password(credentials.Password).Hash;

            if(intentedUser.Password.Hash != intendedHash)
            {
                return Unauthorized();
            }
            
            var myclaims = new List<Claim>(new Claim[] 
            { 
                new Claim(ClaimTypes.NameIdentifier, intentedUser.Id.ToString()),
                new Claim(ClaimTypes.Role, intentedUser.Role.ToString())
            });


            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(myclaims, "WebCookieAuthMiddleware"));

            //HttpContext.Authentication.SignInAsync("WebCookieAuthMiddleware", claimsPrincipal).Wait();
            HttpContext.SignInAsync("WebCookieAuthMiddleware", claimsPrincipal).Wait();
            return Ok(UserMapper.UserToUserModel(intentedUser));
        }

        [HttpPost("user/logout")]
        [Authorize("User")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("WebCookieAuthMiddleware");
            return Ok();
        }

        [HttpGet("user/current")]
        [Authorize("User")]
        public IActionResult Current()
        {
            var currentUserId = User.GetId();

            var currentUser = _userManager.GetUserById(currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return Ok(UserMapper.UserToUserModel(currentUser));
        }
    }
}