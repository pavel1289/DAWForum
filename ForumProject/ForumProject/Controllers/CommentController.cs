using ForumProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }


    }
}