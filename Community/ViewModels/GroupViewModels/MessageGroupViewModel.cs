using System;

namespace Community.ViewModels.GroupViewModels
{
    public class MessageGroupViewModel
    {
        public string CurrentUserName { get; set; }
        public string MessageCreatorName { get; set; }
        public string MessageText { get; set; }
        public int GroupId { get; set; }
        public DateTime MessageTimeCreated { get; set; }
    }
}