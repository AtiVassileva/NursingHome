using System.ComponentModel.DataAnnotations;
using static NursingHome.DAL.Common.ModelConstants;

namespace NursingHome.UI.Models
{
    public class MessageCreateViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Моля, въведете заглавие!")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете съдържание!")]
        [Display(Name = "Съдържание")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Моля, изберете целева аудитория!")]
        [Display(Name = "Целева аудитория")]
        public MessageAudience Audience { get; set; }

        [Display(Name = "Прикачен файл")]
        public IFormFile? Attachment { get; set; }
    }
}