using MongoDB.Bson;

namespace Common.Entities
{
    public class Attendance : PersistentEntity
    {
        public ObjectId UserId { get; set; }

        public ObjectId EventId { get; set; }

        public AttendanceType AttendanceType { get; set;}
    }
}