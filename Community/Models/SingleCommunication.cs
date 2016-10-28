using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Community.Models
{
    public class SingleCommunication
    {
        public ApplicationUser GetUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.Find(userId);
            }
        }

        public void SetMessageStatus(MessageStatus status, string userId, int messageId)
        {
            using (var db = new ApplicationDbContext())
            {
                var singleMessage = (from e in db.SingleMessages
                    where e.MessageId == messageId && e.ReceiverId == userId
                    select e).FirstOrDefault();

                if (singleMessage != null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    singleMessage.Status = status;
                    db.SingleMessages.Attach(singleMessage);
                    db.Entry(singleMessage).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public bool Send(string userId, string receiverNames, Message message)
        {
            var matches = Regex.Matches(receiverNames, @"[A-Öa-ö0-9-_]+");
            var list = matches.Cast<Match>().Select(match => match.Value).ToList();
            var noDupes = list.Distinct().ToList();

            using (var db = new ApplicationDbContext())
            {
                var userSender = db.Users.Find(userId);
                foreach (var name in noDupes)
                {
                    var userRecevier = db.Users.FirstOrDefault(x => x.UserName.Equals(name));

                    if (userRecevier == null)
                    {
                        return false;
                    }
                    var singleMessage = new SingleMessage
                    {
                        Sender = userSender,
                        Receiver = userRecevier,
                        Message = message,
                        Status = MessageStatus.Unread
                    };
                    db.SingleMessages.Add(singleMessage);
                    db.Messages.Add(message);
                }
                db.SaveChanges();
            }
            return true;
        }

        //Eager Loading
        public List<SingleMessage> GetAllUndeletedSingleMessages(string userId)
        {
            List<SingleMessage> singleMessagesList;

            using (var db = new ApplicationDbContext())
            {
                singleMessagesList =
                    (from x in db.SingleMessages
                        where x.ReceiverId == userId && x.Status != MessageStatus.Deleted
                        select x).
                        Include(m => m.Message).
                        Include(u => u.Sender).
                        ToList();
            }

            return singleMessagesList;
        }

        public Message GetMessage(int messageId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Messages.Find(messageId);
            }
        }

        public int GetNrOfUnreadMessages(string userId)
        {
            int unreadMessages;
            using (var db = new ApplicationDbContext())
            {
                unreadMessages =
                    (from x in db.SingleMessages
                        where x.ReceiverId == userId && x.Status == MessageStatus.Unread
                        select x).Count();
            }
            return unreadMessages;
        }
    }
}