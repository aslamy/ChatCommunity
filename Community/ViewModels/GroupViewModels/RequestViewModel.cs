using System.ComponentModel.DataAnnotations;

namespace Community.ViewModels.GroupViewModels
{
    public class RequestViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int MemberId { get; set; }
        public int GroupId { get; set; }
    }
}