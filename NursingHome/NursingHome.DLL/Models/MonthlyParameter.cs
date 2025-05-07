using System.ComponentModel.DataAnnotations;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.DAL.Models
{
    public class MonthlyParameter : BaseEntity
    {
        [Required]
        [Display(Name = "Месец")]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Година")]
        public int Year { get; set; }

        [Display(Name = "Брой дни в месеца")]
        public int DaysInMonth => DateTime.DaysInMonth(Year, Month);

        public List<StayRate> StayRates { get; set; } = new();
               
        public List<DietRate> DietRates { get; set; } = new();

        public static MonthlyParameter CreateInstance(int year, int month)
        {
            var instance = new MonthlyParameter
            {
                Year = year,
                Month = month,
                StayRates = Enum.GetValues<RoomType>()
                    .Select(rt => new StayRate {RoomType = rt})
                    .ToList(),
                DietRates = Enum.GetValues<DietNumber>()
                    .Select(dt => new DietRate {DietNumber = dt})
                    .ToList()
            };

            return instance;
        }
    }
}