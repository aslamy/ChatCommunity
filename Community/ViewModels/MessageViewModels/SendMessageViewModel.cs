using System.ComponentModel.DataAnnotations;

namespace Community.ViewModels.MessageViewModels
{
    public class SendMessageViewModel
    {
        [Required]
        [Display(Name = "To")]
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subjcet { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}