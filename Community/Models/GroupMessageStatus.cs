using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community.Models
{
    public class GroupMessageStatus
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("GroupMessage")]
        public int GroupMessageId { get; set; }

        public bool IsRead { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public virtual GroupMessage GroupMessage { get; set; }
    }
}