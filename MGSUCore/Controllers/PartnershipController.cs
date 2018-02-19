using System;
using System.Linq;
using Common.Entities;
using MGSUCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PartnershipManagment;

namespace MGSUCore.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class PartnershipController : Controller
    {
        private readonly IPartnershipManager _partnershipManager;

        public PartnershipController(IPartnershipManager partnershipManager)
        {
            _partnershipManager = partnershipManager;
        }

        [HttpGet]
        public IActionResult GetAllPartnerships()
        {
            return Ok(_partnershipManager.GetByPredicate(partnership => !partnership.IsDeleted).Select(
                partnership => new
                {
                    Id = partnership.Id.ToString(),
                    partnership.Direction,
                    partnership.Description
                }));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPartnershipById([FromRoute] string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");
            var partnership = _partnershipManager.GetById(objectId);
            if (partnership is null)
            {
                return NotFound();
            }
            return Ok(partnership);
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult CreatePartnership([FromBody] PartnershipDTO partnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var partnership = new Partnership()
            {
                Direction = partnershipModel.Direction,
                CreatingDate = DateTime.Now,
                IsDeleted = false,
                Description = partnershipModel.Description
            };
            return Ok(_partnershipManager.CreatePartnership(partnership).ToString());
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize("Admin")]
        public IActionResult UpdatePartnership([FromRoute] string id ,[FromBody] PartnershipDTO partnership)
        {
            if (!ModelState.IsValid || !ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest(ModelState);
            }
            var oldPartnership = _partnershipManager.GetById(objectId);
            if (oldPartnership is null)
            {
                return NotFound();
            }
            oldPartnership = partnership.GetEntity(id);
            _partnershipManager.UpdatePartnership(oldPartnership);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize("Admin")]
        public IActionResult DeletePartnership([FromRoute] string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");
            var oldPartnership = _partnershipManager.GetById(objectId);
            if (oldPartnership is null)
            {
                return NotFound();
            }
            _partnershipManager.DeletePartnership(objectId);
            return Ok(id);
        }
    }
}