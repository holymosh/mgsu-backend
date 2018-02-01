using System;

namespace Common.Entities
{
    public class UserProfile
    {
        public DateTimeOffset UniversityGraduationYaer { get;  set; }
        public DateTimeOffset Birthyear { get;  set; }
        public string InstitutionName { get;  set; }
    }
}