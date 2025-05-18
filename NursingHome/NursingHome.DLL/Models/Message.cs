using System.ComponentModel.DataAnnotations;

namespace NursingHome.DAL.Models
{
    using static Common.ModelConstants;

    public class Message : BaseEntity
    {
        [Required(ErrorMessage = "Моля, въведете заглавие!")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете съдържание!")]
        [Display(Name = "Съдържание")]
        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public MessageAudience Audience { get; set; }

        public string? AttachmentFileName { get; set; }

        public string? AttachmentFilePath { get; set; }

        [Required]
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Sender { get; set; } = null!;
    }
}