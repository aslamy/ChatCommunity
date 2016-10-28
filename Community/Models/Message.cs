using System;
using System.Collections.Generic;

namespace Community.Models
{
    public class Message
    {
        public Message()
        {
            GroupMessages = new List<GroupMessage>();
            SingleMessages = new List<SingleMessage>();

            //   UserMessageStatuses = new List<UserMessageStatus>();
        }

        public int Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime TimeCreated { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
        public virtual ICollection<SingleMessage> SingleMessages { get; set; }
    }
}