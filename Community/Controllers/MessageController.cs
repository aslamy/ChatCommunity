using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Community.Models;
using Community.ViewModels.MessageViewModels;
using Microsoft.AspNet.Identity;

namespace Community.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        public static List<List<InboxViewModel>> AllMessages { get; private set; }
        // GET: Message
        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var messageToSend = new Message
            {
                Subject = model.Subjcet,
                Text = model.Message,
                TimeCreated = DateTime.Now
            };
            var com = new SingleCommunication();
            var isSend = com.Send(userId, model.ReceiverName, messageToSend);
            if (!isSend)
            {
                TempData["errorMessage"] = "Invalid user name!";
                return View(model);
            }
            ModelState.Clear();
            TempData["successMessage"] = "Your message is send!";
            return View();
        }

        public ActionResult Inbox()
        {
            return View();
        }

        public PartialViewResult _NewMessages()
        {
            AllMessages = GetAllUndeletedMessages();

            return PartialView(AllMessages);
        }

        public PartialViewResult _Detail(int id)
        {
            ViewBag.detailID = id;
            return PartialView(AllMessages[id]);
        }

        public PartialViewResult _Read(int id)
        {
            var userId = User.Identity.GetUserId();
            var com = new SingleCommunication();
            var message = com.GetMessage(id);
            com.SetMessageStatus(MessageStatus.Read, userId, id);
            var model = new MessageViewModel();
            if (message != null)
            {
                foreach (var mes in from @group in AllMessages from mes in @group where mes.MessageId == id select mes)
                {
                    mes.Status = MessageStatus.Read;
                }
                model.Id = id;
                model.Subject = message.Subject;
                model.Text = message.Text;
                model.TimeCreated = message.TimeCreated;
            }
            return PartialView(model);
        }

        [HttpPost]
        public void DeleteMessage(int id)
        {
            var userId = User.Identity.GetUserId();
            var com = new SingleCommunication();
            com.SetMessageStatus(MessageStatus.Deleted, userId, id);

            foreach (
                var message in
                    from @group in AllMessages from message in @group where message.MessageId == id select message)
            {
                message.Status = MessageStatus.Deleted;
            }
        }

        private List<List<InboxViewModel>> GetAllUndeletedMessages()
        {
            var list = new List<InboxViewModel>();
            var userId = User.Identity.GetUserId();

            var com = new SingleCommunication();
            var allSingleMessages = com.GetAllUndeletedSingleMessages(userId);
            foreach (var sm in allSingleMessages)
            {
                var model = new InboxViewModel
                {
                    Subjcet = sm.Message.Subject,
                    MessageId = sm.Message.Id,
                    Date = sm.Message.TimeCreated,
                    Username = sm.Sender.UserName,
                    FirstName = sm.Sender.FirstName,
                    LastName = sm.Sender.LastName,
                    Status = sm.Status
                };
                list.Add(model);
            }
            var groupedList = list
                .OrderByDescending(x => x.Date)
                .GroupBy(u => u.Username)
                .Select(grp => grp.ToList())
                .ToList();
            return groupedList;
        }
    }
}