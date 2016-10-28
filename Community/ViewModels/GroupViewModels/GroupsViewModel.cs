using System;
using System.ComponentModel.DataAnnotations;
using Community.Models;

namespace Community.ViewModels.GroupViewModels
{
    public class GroupsViewModel
    {
        [Required]
        [Display(Name = "Group Id")]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Group Created at")]
        public DateTime TimeCreated { get; set; }

        [Display(Name = "Administrator")]
        public string Administrator { get; set; }

        [Display(Name = "Member since")]
        public DateTime MemberShip { get; set; }

        [Display(Name = "Member status")]
        public string MemberStatus { get; set; }

        public GroupStatus? GroupStatus { get; set; }
    }
}