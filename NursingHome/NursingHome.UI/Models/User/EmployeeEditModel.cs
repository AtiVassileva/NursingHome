using static NursingHome.DAL.Common.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace NursingHome.UI.Models.User
{
    public class EmployeeEditModel : BaseEditModel
    {
        [Display(Name = "Длъжност")]
        public EmployeePosition EmployeePosition { get; set; }
    }
}