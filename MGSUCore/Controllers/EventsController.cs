using System;
using System.Linq;
using Common.Entities;
using EventManagment;
using MGSUCore.Authentification;
using MGSUCore.Filters;
using MGSUCore.Models;
using MGSUCore.Models.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MGSUCore.Controllers
{
    [CustomExceptionFilterAttribute]
    [Route("[controller]")]
    public class EventsController : Controller
    {
        private readonly IAttendanceManager _attendanceManager;
        private readonly IEventManager _eventmanager;

        public EventsController(IEventManager eventManager, IAttendanceManager attendanceManager)
        {
            _eventmanager = eventManager;
            _attendanceManager = attendanceManager;
        }

        [HttpGet("{year}/{month}")]
        [Authorize("User")]
        public IActionResult GetEventsByYearAndMonth(int year, int month)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(month > 12)
                return BadRequest("Month is between 1 and 12 inclusive");

            var beginOfMonth = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeZoneInfo.Local.BaseUtcOffset);
            var beginOfNextMonth = beginOfMonth.AddMonths(1);

            var allAttendancesOfUser = _attendanceManager.GetAllIdsOfUserEvents(User.GetId());

            var eventsForUser = allAttendancesOfUser.Select(attendance => _eventmanager.GetEventById(attendance.EventId));

            var eventsToReturn = eventsForUser.Where(
                    post => post.Date >= beginOfMonth &&
                            post.Date < beginOfNextMonth);

            return Ok(eventsToReturn.Select(EventMapper.EventToEventModel));
        }

        [HttpPost("attend/{eventIdString}")]
        [Authorize("User")]
        public IActionResult AttendUserToEvend(string eventIdString, [FromBody]int newAttendanceString)
        {
            var userId = User.GetId();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!ObjectId.TryParse(eventIdString, out var eventId))
                return BadRequest("'EventId' parameter is ivalid ObjectId");

            var eventToAttend = _eventmanager.GetEventById(eventId);
            
            if(eventToAttend == null)
                return NotFound();

            var currentAttendanceType = _attendanceManager.GetAttendanceType(userId, eventId);

            var newAttendanceType = (AttendanceType)newAttendanceString;

            if(currentAttendanceType == (AttendanceType)newAttendanceType)
            {
                return Ok("Attendace type not modified");
            }

            _attendanceManager.ChangeAttendanceType(userId, eventId, newAttendanceType);
            
            return Ok("Attendace type changed");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return
                Ok(_eventmanager.GetEventsByPredicate(@event => @event.Date > DateTimeOffset.Now)
                                                                .Select(EventMapper.EventToEventModel));

        }

        // GET: events/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var eventToReturn = _eventmanager.GetEventById(objectId);

            if (eventToReturn == null)
                return NotFound();

            return Ok(EventMapper.EventToEventModel(eventToReturn));
        }

        // POST: events
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] EventSavingModel eventSavingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var eventToCreate = EventMapper.EventModelToEvent(eventSavingModel);

            return Ok(_eventmanager.CreateEvent(eventToCreate).ToString());
        }

        // PUT: events/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult Put(string id, [FromBody] EventSavingModel eventSavingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldEvent = _eventmanager.GetEventById(objectId);

            if (oldEvent == null)
                return NotFound();

            oldEvent.Title = eventSavingModel.Title ?? oldEvent.Title;
            oldEvent.Content = eventSavingModel.Content ?? oldEvent.Content;
            oldEvent.Date = eventSavingModel.Date;
            oldEvent.Description = eventSavingModel.Description ?? oldEvent.Description;

            _eventmanager.UpdateEvent(oldEvent);
            return Ok(id);
        }

        // DELETE: events/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult Delete(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldEvent = _eventmanager.GetEventById(objectId);

            if (oldEvent == null)
                return NotFound();

            _eventmanager.DeleteEvent(objectId);
            return Ok(id);
        }
    }
}