using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Entities;
using MongoDB.Bson;

namespace EventManagment
{
    public interface IEventManager
    {
         Event GetEventById(ObjectId eventId);
         IEnumerable<Event> GetEventsByPredicate(Expression<Func<Event, bool>> predicate = null);
         ObjectId CreateEvent(Event eventToCreate);
         void UpdateEvent(Event eventToUpdate);
         void DeleteEvent(ObjectId eventId);
    }
}