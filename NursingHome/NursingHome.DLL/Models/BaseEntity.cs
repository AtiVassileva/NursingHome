using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    public class BaseEntity
    {
        [Required] public Guid Id { get; set; }
    }
}