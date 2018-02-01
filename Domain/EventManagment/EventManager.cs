using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using Common.Entities;
using DataAccess.Application;
using MongoDB.Bson;

namespace EventManagment
{
    public class EventManager : IEventManager
    {
        private readonly IRepository<Event> _eventRepository;
        public EventManager(IRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public ObjectId CreateEvent(Event eventToCreate)
        {
            Require.NotNull(eventToCreate, nameof(eventToCreate));

            return _eventRepository.Create(eventToCreate);
        }

        public void DeleteEvent(ObjectId eventId)
        {
            Require.NotNull(eventId, nameof(eventId));

            _eventRepository.Delete(eventId);
        }

        public Event GetEventById(ObjectId eventId)
        {
            Require.NotNull(eventId, nameof(eventId));

            return _eventRepository.GetById(eventId);
        }

        public IEnumerable<Event> GetEventsByPredicate(Expression<Func<Event, bool>> predicate = null)
        {
            return _eventRepository.GetByPredicate(predicate);
        }

        public void UpdateEvent(Event eventToUpdate)
        {
            Require.NotNull(eventToUpdate, nameof(eventToUpdate));

            _eventRepository.Update(eventToUpdate);
        }
    }
}