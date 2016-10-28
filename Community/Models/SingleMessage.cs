using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community.Models
{
    public class SingleMessage
    {
        public int Id { get; set; }

        [Index("IX_MessageAndSenderIdAndReceiver", 1, IsUnique = true)]
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        [Index("IX_MessageAndSenderIdAndReceiver", 2, IsUnique = true)]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        [Index("IX_MessageAndSenderIdAndReceiver", 3, IsUnique = true)]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        [Required]
        public MessageStatus Status { get; set; }

        [Required]
        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public virtual Message Message { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
    }
}