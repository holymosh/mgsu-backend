using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UserManagment.Application;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using MGSUCore.Filters;
using System.Linq;
using MGSUCore.Models.Mappers;
using Newtonsoft.Json;
using MGSUCore.Models.Convertors;
using MGSUCore.Models;

namespace MGSUCore.Controllers
{
    [CustomExceptionFilterAttribute]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private readonly IContactManager _contactManager;

        public ContactsController(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }

        // GET: api/Contacts
        [HttpGet]
        public IActionResult Get()
        {
            if (!Request.Query.TryGetValue("team", out Microsoft.Extensions.Primitives.StringValues value))
            {
                return Ok(_contactManager.GetContactByPredicate().Select(ContactMapper.ContactToContactModel));
            }

            if (value.Count > 1)
            {
                return BadRequest("Query bad");
            }

            var team = value.Single();

            var contactsToReturn = _contactManager.GetContactByPredicate(contact => contact.Team == team);
            
            return
                Ok(contactsToReturn.Select(ContactMapper.ContactToContactModel));
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId))
 
                return BadRequest("'Id' parameter is ivalid ObjectId");
 
            var contact = _contactManager.GetContactById(objectId);

            if (contact == null)
                return NotFound();

            return Ok(ContactMapper.ContactToContactModel(contact));
        }

        // POST: api/Contacts
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] ContactSavingModel contactToCreateModel, string team)
        {
            if(team != null)
            {
                var teamName = nameof(contactToCreateModel.Team);
                contactToCreateModel.Team = team;
                ModelState.ClearError(teamName);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactToCreate = ContactMapper.ContactModelToContact(contactToCreateModel);

            return Ok(_contactManager.CreateContact(contactToCreate).ToString());
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult Put(string id, [FromBody] ContactSavingModel contactToUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if(!ObjectId.TryParse(id, out var objectId))
 
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldContact = _contactManager.GetContactById(objectId);

            if (oldContact == null)
                return NotFound();

            oldContact.Description = contactToUpdate.Description ?? oldContact.Description;
            oldContact.FirstName = contactToUpdate.FirstName ?? oldContact.FirstName;
            oldContact.LastName = contactToUpdate.LastName ?? oldContact.LastName;
            oldContact.MiddleName = contactToUpdate.MiddleName ?? oldContact.MiddleName;
            oldContact.Img = contactToUpdate.Img == null ? oldContact.Img : new Image{
                Original = contactToUpdate.Img.Original,
                Small = contactToUpdate.Img.Small,
                Role = contactToUpdate.Img.Role};
            oldContact.Team = contactToUpdate.Team ?? oldContact.Team;

            _contactManager.UpdateContact(oldContact);
            return Ok(id);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult Delete(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldContact = _contactManager.GetContactById(objectId);

            if (oldContact == null)
                return NotFound();

            _contactManager.DeleteContact(objectId);

            return Ok(id);
        }
    }
}