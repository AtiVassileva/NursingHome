namespace NursingHome.DAL.Common
{
    public class ModelConstants
    {
        public enum RoomType
        {
            ApartmentSingle = 1,
            ApartmentDouble = 2,
            ApartmentTriple = 3,
            StudioSingle = 4,
            StudioDouble = 5,
            StudioTriple = 6,
            DoubleRoomSingle = 7,
            DoubleRoomDouble = 8
        }

        public enum DietNumber
        {
            Diet1 = 1,
            Diet9 = 9,
            Diet10 = 10,
            Diet15 = 15
        }

        public enum UserStatus
        {
            Active = 1,
            Inactive = 2
        }

        public enum Gender
        {
            Male = 1,
            Female = 2
        }

        public enum EmployeePosition
        {
            Cashier = 1,
            OccupationalTherapist = 2,
            Psychologist = 3,
            SocialWorker = 4,
            Cook = 5
        }
    }
}