using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community.Models
{
    public class GroupMember
    {
        public int Id { get; set; }

        [Index("IX_UserAndGroupAndStatus", 3, IsUnique = true)]
        public GroupStatus? Status { get; set; }

        [Index("IX_UserAndGroupAndStatus", 1, IsUnique = true)]
        [ForeignKey("User")]
        public string MemberId { get; set; }

        [Index("IX_UserAndGroupAndStatus", 2, IsUnique = true)]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public DateTime MembershipTime { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public virtual Group Group { get; set; }
    }
}