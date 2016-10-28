using System.ComponentModel.DataAnnotations;

namespace Community.ViewModels.GroupViewModels
{
    public class CreateGroupViewModel
    {
        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Members")]
        public string Members { get; set; }
    }
}