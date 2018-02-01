using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Entities;
using Common.Exceptions;
using DataAccess.Application;
using MongoDB.Bson;

namespace EventManagment
{
    public class AttendanceManager : IAttendanceManager
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        public AttendanceManager(IRepository<Attendance> attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public ObjectId AttendUserToEvent(ObjectId userId, ObjectId eventId, AttendanceType attendanceType)
        {
            Require.NotNull(userId, nameof(userId));
            Require.NotNull(eventId, nameof(eventId));

            var sameAttendances = _attendanceRepository.GetByPredicate(attendance => 
                                                                attendance.UserId == userId &&
                                                                attendance.EventId == eventId);
            if(sameAttendances.Count() > 1)
            {
                throw new PolicyException("More than one attendance of same user to same event");
            }

            if(sameAttendances.Count() == 1)
            {
                ChangeAttendanceType(userId, eventId, attendanceType);
            }

            return _attendanceRepository.Create(new Attendance
            {
                UserId = userId, 
                EventId = eventId,
                AttendanceType = attendanceType
            });

        }

        public void ChangeAttendanceType(ObjectId userId, ObjectId eventId, AttendanceType newAttendanceType)
        {
            Require.NotNull(userId, nameof(userId));
            Require.NotNull(eventId, nameof(eventId));

            var sameAttendances = _attendanceRepository.GetByPredicate(attendance => 
                                                                attendance.UserId == userId &&
                                                                attendance.EventId == eventId);
            if(sameAttendances.Count() != 1)
            {
                throw new PolicyException("There are no one attendance on this event for this user");
            }

            var attendanceToUpdate = sameAttendances.Single();

            attendanceToUpdate.AttendanceType = newAttendanceType;

            _attendanceRepository.Update(attendanceToUpdate);
        }

        public IEnumerable<Attendance> GetAllIdsOfUserEvents(ObjectId userId)
        {
            Require.NotNull(userId, nameof(userId));

            return _attendanceRepository.GetByPredicate(attendance => attendance.UserId == userId && 
                                                            attendance.AttendanceType != AttendanceType.NotGoing);
        }

        public AttendanceType GetAttendanceType(ObjectId userId, ObjectId eventId)
        {
            Require.NotNull(userId, nameof(userId));
            Require.NotNull(eventId, nameof(eventId));

            var userAttendance = _attendanceRepository.GetByPredicate(attendance => 
                                                        attendance.UserId == userId && 
                                                        attendance.EventId == eventId).SingleOrDefault();
            if(userAttendance == null)
            {
                _attendanceRepository.Create(new Attendance
                {
                    UserId = userId,
                    EventId = eventId,
                    AttendanceType = AttendanceType.NotGoing
                });

                return AttendanceType.NotGoing;
            }

            return userAttendance.AttendanceType;
        }
    }
}