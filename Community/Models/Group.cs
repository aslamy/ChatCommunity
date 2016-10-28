using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community.Models
{
    public class Group
    {
        public Group()
        {
            GroupMembers = new List<GroupMember>();
            GroupMessages = new List<GroupMessage>();
        }

        public int Id { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(450)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
    }
}