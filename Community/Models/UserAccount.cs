using System.Linq;
using System.Text.RegularExpressions;

namespace Community.Models
{
    public class UserAccount
    {
        public bool UsersExist(string names)
        {
            var matches = Regex.Matches(names, @"[A-Öa-ö0-9-_]+");
            var list = matches.Cast<Match>().Select(match => match.Value).ToList();
            var noDupes = list.Distinct().ToList();
            using (var db = new ApplicationDbContext())
            {
                foreach (var name in noDupes)
                {
                    var userRecevier = db.Users.FirstOrDefault(x => x.UserName.Equals(name));

                    if (userRecevier == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}