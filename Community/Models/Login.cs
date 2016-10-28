using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community.Models
{
    public class Login
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}