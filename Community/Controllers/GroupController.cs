using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Community.Models;
using Community.ViewModels.GroupViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Community.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SendGroupMessage(int groupId, string messageText)
        {
            var com = new GroupCommunication();
            var userId = User.Identity.GetUserId();
            var message = new Message
            {
                Text = messageText,
                TimeCreated = DateTime.Now
            };
            com.SendGroupMessage(userId, message, groupId);

            return _GroupMessages(groupId);
        }

        [HttpGet]
        public PartialViewResult _Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public PartialViewResult _Create(CreateGroupViewModel model)
        {
            string userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }
            var useraccount = new UserAccount();

            if (!model.Members.IsNullOrWhiteSpace())
            {
                var userExist = useraccount.UsersExist(model.Members);
                if (!userExist)
                {
                    TempData["errorMessage"] = "Invalid user name!";
                    return PartialView("_Create", model);
                }
            }
            var groupcom = new GroupCommunication();

            if (groupcom.GroupExist(model.GroupName))
            {
                TempData["errorMessage"] = "This group name is already taken!";
                return PartialView("_Create", model);
            }

            string members = model.Members + ";" + User.Identity.Name;
            groupcom.SaveNewGroup(userId, model.GroupName, model.Description, members, DateTime.Now);
            ModelState.Clear();
            TempData["successMessage"] = "This group is Created!";

            return PartialView("_Create");
        }

        [HttpGet]
        public PartialViewResult _ShowMyGroups()
        {
            var modelList = new List<GroupsViewModel>();
            var useId = User.Identity.GetUserId();
            var groupcom = new GroupCommunication();
            var mygroups = groupcom.GetUsersGroups(useId);
            foreach (var group in mygroups)
            {
                var model = new GroupsViewModel
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    Description = group.Description,
                    TimeCreated = group.TimeCreated
                };

                modelList.Add(model);
            }
            return PartialView("_ShowMyGroups", modelList);
        }

        public PartialViewResult _SearchGroups()
        {
            return PartialView("_SearchGroups");
        }

        [HttpPost]
        public PartialViewResult _Group(int id)
        {
            return PartialView("_Group", id);
        }

        public PartialViewResult _GroupInformation(int id)
        {
            var userId = User.Identity.GetUserId();
            var com = new GroupCommunication();

            var group = com.GetGroup(id);

            var member = com.GetGroupMember(userId, id);

            var name = group.Creator.FirstName + " " + group.Creator.LastName;
            var model = new GroupsViewModel
            {
                GroupName = group.Name,
                Administrator = name,
                Description = group.Description,
                TimeCreated = group.TimeCreated,
                MemberShip = member.MembershipTime
            };
            if (group.Creator.UserName.Equals(User.Identity.GetUserName()))
            {
                model.MemberStatus = "Administrator";
            }
            else
            {
                model.MemberStatus = "User";
            }

            return PartialView("_GroupInformation", model);
        }

        public PartialViewResult _GroupMembers(int id)
        {
            var com = new GroupCommunication();
            var model = new List<string>();
            var members = com.GetAllGroupMembers(id, GroupStatus.Accepted);
            foreach (var member in members)
            {
                var name = member.User.FirstName + " " + member.User.LastName;
                model.Add(name);
            }
            return PartialView("_GroupMembers", model);
        }

        public PartialViewResult _GroupRequests(int id)
        {
            var model = new List<RequestViewModel>();
            var userId = User.Identity.GetUserId();
            var com = new GroupCommunication();
            var group = com.GetUsersGroups(userId).FirstOrDefault(x => x.Id == id);

            if (group != null)
            {
                var members = com.GetAllGroupMembers(group.Id, GroupStatus.Waiting);
                foreach (var member in members)
                {
                    if (member.Group.CreatorId == userId)
                    {
                        var requset = new RequestViewModel
                        {
                            UserName = member.User.UserName,
                            FirstName = member.User.FirstName,
                            LastName = member.User.LastName,
                            Email = member.User.Email,
                            MemberId = member.Id,
                            GroupId = group.Id
                        };

                        model.Add(requset);
                    }
                }
            }


            return PartialView("_GroupRequests", model);
        }

        public void SendGroupRequest(int id)
        {
            var com = new GroupCommunication();
            var userId = User.Identity.GetUserId();
            com.SetGroupRequestStatus(userId, id, GroupStatus.Waiting);
        }

        public PartialViewResult _GroupMessages(int id)
        {
            var model = new List<MessageGroupViewModel>();
            var com = new GroupCommunication();

            var currentUser = com.GetUser(User.Identity.GetUserId());


            var currentUserName = currentUser.FirstName + " " + currentUser.LastName;

            var membershipTime = com.GetGroupMember(User.Identity.GetUserId(), id).MembershipTime;


            var groupMessages = com.GetGroupMessages(id, membershipTime);

            foreach (var groupMessage in groupMessages)
            {
                var messageCreatorName = groupMessage.User.FirstName + " " + groupMessage.User.LastName;
                var messagegGroupModel = new MessageGroupViewModel
                {
                    GroupId = id,
                    MessageText = groupMessage.Message.Text,
                    MessageTimeCreated = groupMessage.Message.TimeCreated,
                    CurrentUserName = currentUserName,
                    MessageCreatorName = messageCreatorName
                };
                model.Add(messagegGroupModel);
            }
            var orderedmodel = model.OrderBy(x => x.MessageTimeCreated).ToList();
            ViewBag.groupId = id;

            return PartialView("_GroupMessage", orderedmodel);
        }

        public PartialViewResult _SearchGroupResult(string id, string keyword)
        {
            if (id != null)
            {
                SendGroupRequest(int.Parse(id));
            }

            var modelList = new List<GroupsViewModel>();
            var userId = User.Identity.GetUserId();
            var com = new GroupCommunication();
            List<Group> mygroups;
            if (keyword == null)
            {
                ViewBag.keyword = null;
                mygroups = com.GetAllGroups();
            }
            else
            {
                ViewBag.keyword = keyword;
                mygroups = com.GetGroups(keyword);
            }


            foreach (var group in mygroups)
            {
                var model = new GroupsViewModel
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    Description = group.Description,
                    TimeCreated = group.TimeCreated
                };
                var member = com.GetGroupMember(userId, group.Id);
                if (member != null) model.GroupStatus = member.Status;
                modelList.Add(model);
            }
            return PartialView("_ShowGroups", modelList);
        }

        public void AcceptGroupRequest(int id)
        {
            var com = new GroupCommunication();
            com.ChangeMemberStatus(id, GroupStatus.Accepted);
        }

        public void DenyGroupRequest(int id)
        {
            var com = new GroupCommunication();
            com.ChangeMemberStatus(id, GroupStatus.Denied);
        }
    }
}