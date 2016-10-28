using System;

namespace Community.ViewModels.MessageViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime? TimeCreated { get; set; }
    }
}