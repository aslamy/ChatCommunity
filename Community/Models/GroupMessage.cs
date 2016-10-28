using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community.Models
{
    public class GroupMessage
    {
        public GroupMessage()
        {
            GroupMessageStatuses = new List<GroupMessageStatus>();
        }

        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Message")]
        public int MessageId { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public virtual Message Message { get; set; }
        public virtual Group Group { get; set; }
        public DateTime CreatedTime { get; set; }
        public virtual ICollection<GroupMessageStatus> GroupMessageStatuses { get; set; }
    }
}