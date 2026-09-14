using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public static class SeedDataEnum
    {
        public enum UserTypeEnum
        {
            Parent = 1,
            Teacher = 2,
            Admin = 3
        }
        public enum GenderEnum
        {
            Male = 1,
            Female = 2,
            Unknown=3
        }
     
    }
}
