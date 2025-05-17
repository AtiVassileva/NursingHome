using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Common
{
    public class ModelConstants
    {
        public enum RoomType
        {
            [Display(Name = "Апартамент сам")]
            ApartmentSingle = 1,
            [Display(Name = "Апартамент двама")]
            ApartmentDouble = 2,
            [Display(Name = "Апартамент трима")]
            ApartmentTriple = 3,
            [Display(Name = "Гарсониера сам")]
            StudioSingle = 4,
            [Display(Name = "Гарсониера двама")]
            StudioDouble = 5,
            [Display(Name = "Гарсониера трима")]
            StudioTriple = 6,
            [Display(Name = "Двойна стая сам")]
            DoubleRoomSingle = 7,
            [Display(Name = "Двойна стая двама")]
            DoubleRoomDouble = 8
        }

        public enum DietNumber
        {
            [Display(Name = "Диета 1")]
            Diet1 = 1,
            [Display(Name = "Диета 9")]
            Diet9 = 9,
            [Display(Name = "Диета 10")]
            Diet10 = 10,
            [Display(Name = "Диета 15")]
            Diet15 = 15
        }

        public enum UserStatus
        {
            [Display(Name = "Активен")]
            Active = 1,
            [Display(Name = "Неактивен")]
            Inactive = 2
        }

        public enum Gender
        {
            [Display(Name = "Мъж")]
            Male = 1,
            [Display(Name = "Жена")]
            Female = 2
        }

        public enum EmployeePosition
        {
            [Display(Name = "Касиер - домакин")]
            Cashier = 1,
            [Display(Name = "Трудотерапевт")]
            OccupationalTherapist = 2,
            [Display(Name = "Психолог")]
            Psychologist = 3,
            [Display(Name = "Социален работник")]
            SocialWorker = 4,
            [Display(Name = "Работник кухня")]
            Cook = 5,
            [Display(Name = "Медицинска сестра")]
            Nurse = 6
        }

        public enum PaymentStatus
        {
            [Display(Name = "Платено")]
            Paid = 1,
            [Display(Name = "Неплатено")]
            Unpaid = 2
        }

        public enum ReportType
        {
            [Display(Name = "Месечен")]
            Monthly = 1,
            [Display(Name = "Годишен")]
            Yearly = 2
        }
    }
}