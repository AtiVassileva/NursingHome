using System.ComponentModel.DataAnnotations;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Models.User
{
    public class EmployeeRegisterModel
    {
        [Display(Name = "Длъжност")]
        public EmployeePosition EmployeePosition { get; set; }
    }
}