using ForumProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class DiscussionThreadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DiscussionThread
        public ActionResult Index()
        {
            return View();
        }
    }
}