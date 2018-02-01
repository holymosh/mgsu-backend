using System.Collections.Generic;
using Common.Entities;
using MongoDB.Bson;

namespace EventManagment
{
    public interface IAttendanceManager
    {
         ObjectId AttendUserToEvent(ObjectId userId, ObjectId eventId, AttendanceType attendanceType);
         void ChangeAttendanceType(ObjectId userId, ObjectId eventId, AttendanceType attendanceType);
         AttendanceType GetAttendanceType(ObjectId userId, ObjectId eventId);

         IEnumerable<Attendance> GetAllIdsOfUserEvents(ObjectId userId);
    }
}