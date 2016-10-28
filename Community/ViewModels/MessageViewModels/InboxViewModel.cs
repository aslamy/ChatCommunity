using System;
using Community.Models;

namespace Community.ViewModels.MessageViewModels
{
    public class InboxViewModel
    {
        public int MessageId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subjcet { get; set; }
        public DateTime Date { get; set; }
        public MessageStatus Status { get; set; }
    }
}