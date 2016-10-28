using System;
using System.ComponentModel.DataAnnotations;

namespace Community.ViewModels.UserViewModels
{
    public class UserViewModel
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

        [Display(Name = "Number of logins")]
        public static int NrOfLogins { get; set; }

        [Display(Name = "Last time login")]
        public static DateTime? LastTimeLogin { get; set; }

        public int NrOfNewMessages { get; set; }
        public int NrOfNewGroupMessages { get; set; }
    }
}