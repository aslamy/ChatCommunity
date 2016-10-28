using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Community.Models
{
    public class GroupCommunication
    {
        public bool GroupExist(string name)
        {
            using (var db = new ApplicationDbContext())
            {
                var group = db.Groups.FirstOrDefault(x => x.Name.Equals(name));

                if (group == null)
                {
                    return false;
                }
            }
            return true;
        }

        public void SaveNewGroup(string userId, string groupName, string description, string members, DateTime time)
        {
            using (var db = new ApplicationDbContext())
            {
                var group = new Group
                {
                    CreatorId = userId,
                    Name = groupName,
                    TimeCreated = time,
                    Description = description
                };

                var matches = Regex.Matches(members, @"[A-Öa-ö0-9-_]+");
                var list = matches.Cast<Match>().Select(match => match.Value).ToList();
                var noDupes = list.Distinct().ToList();

                foreach (var memberName in noDupes)
                {
                    var member = db.Users.FirstOrDefault(user => user.UserName == memberName);
                    if (member != null)
                    {
                        var memberId = member.Id;
                        var groupMember = new GroupMember
                        {
                            Group = @group,
                            MemberId = memberId,
                            Status = GroupStatus.Accepted,
                            MembershipTime = time
                        };

                        @group.GroupMembers.Add(groupMember);
                    }
                }

                db.Groups.Add(group);
                db.SaveChanges();
            }
        }

        public List<Group> GetUsersGroups(string userId)
        {
            List<Group> groups;
            using (var db = new ApplicationDbContext())
            {
                groups =
                    db.Groups.Where(
                        g => g.GroupMembers.Any(m => m.MemberId == userId && m.Status == GroupStatus.Accepted))
                        .ToList();
            }
            return groups;
        }

        //Eager Loading
        public Group GetGroup(int groupId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Groups.Where(g => g.Id == groupId).
                    Include(u => u.Creator).
                    FirstOrDefault();
            }
        }

        public ApplicationUser GetUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.Find(userId);
            }
        }

        public GroupMember GetGroupMember(string userId, int groupId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.GroupMembers.FirstOrDefault(x => x.MemberId == userId && x.GroupId == groupId);
            }
        }

        // //Eager loading
        public List<GroupMember> GetAllGroupMembers(int groupId, GroupStatus status)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.GroupMembers.Where(x => x.GroupId == groupId && x.Status == status).
                    Include(u => u.User).
                    Include(g => g.Group).
                    ToList();
            }
        }

        public void SetGroupRequestStatus(string userId, int groupId, GroupStatus status)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var groupmember = new GroupMember
                {
                    GroupId = groupId,
                    MemberId = userId,
                    Status = status,
                    MembershipTime = DateTime.Now
                };
                db.GroupMembers.Add(groupmember);
                db.SaveChanges();
            }
        }

        public void ChangeMemberStatus(int memberId, GroupStatus status)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var groupmember = db.GroupMembers.Find(memberId);

                if (groupmember != null)
                {
                    groupmember.Status = status;
                    groupmember.MembershipTime = DateTime.Now;
                    db.Entry(groupmember).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public List<Group> GetAllGroups()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Groups.OrderByDescending(x => x.Name).ToList();
            }
        }

        public List<Group> GetGroups(string keyword)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Groups.Where(x => x.Name.StartsWith(keyword)).OrderByDescending(x => x.Name).ToList();
            }
        }

        ////Eager loading
        public List<GroupMessage> GetGroupMessages(int groupId, DateTime membershipTime)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.GroupMessages.Where(x => x.GroupId == groupId && x.CreatedTime >= membershipTime).
                    Include(m => m.Message).
                    Include(u => u.User).ToList();
            }
        }

        public Message GetMessage(int messageId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Messages.Find(messageId);
            }
        }

        public void SendGroupMessage(string userId, Message message, int groupId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var groupMessage = new GroupMessage
                {
                    GroupId = groupId,
                    UserId = userId,
                    CreatedTime = DateTime.Now
                };
                message.GroupMessages.Add(groupMessage);

                db.Messages.Add(message);
                db.SaveChanges();
            }
        }
    }
}