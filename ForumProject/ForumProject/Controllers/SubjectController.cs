using ForumProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class SubjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Subject
        public ActionResult Index()
        {
            var subjects = from subject in db.Subjects
                           orderby subject.Name
                           select subject;
            ViewBag.Subjects = subjects;
            return View();
        }

        public ActionResult Show(string id)
        {
            Subject subject = db.Subjects.Find(id);
            return View(subject);
        }
    }
}