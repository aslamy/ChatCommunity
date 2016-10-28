using System.Web.Mvc;
using Community.Models;
using Community.ViewModels.UserViewModels;
using Microsoft.AspNet.Identity;

namespace Community.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var com = new SingleCommunication();
            var user = com.GetUser(userId);

            var model = new UserViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                NrOfNewMessages = com.GetNrOfUnreadMessages(userId)
            };

            return View(model);
        }
    }
}