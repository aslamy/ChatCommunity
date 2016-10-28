using System.Collections.Generic;

namespace Community.Models
{
    public class User
    {
        public User()
        {
            SingleMessages = new List<SingleMessage>();
            GroupMembers = new List<GroupMember>();
            GroupMessages = new List<GroupMessage>();
            Groups = new List<Group>();
            GroupMessageStatuses = new List<GroupMessageStatus>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual ICollection<SingleMessage> SingleMessages { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<GroupMessageStatus> GroupMessageStatuses { get; set; }
    }
}