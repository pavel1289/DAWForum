using ForumProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ForumProject.Controllers
{
    public class DiscussionThreadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int id)
        {
            var discussionThreads = from thread in db.DiscussionThreads
                                    orderby thread.Name
                                    where thread.SubjectId == id
                                    select thread;
            foreach(var thread in discussionThreads)
            {
                var userId = thread.UserId;
                thread.User = db.Users.Find(userId);
            }
            ViewBag.DiscussionThreads = discussionThreads;
            ViewBag.CurrentSubject = db.Subjects.Find(id);
            return View();
        }

        public ActionResult Show(int id)
        {
            DiscussionThread discussionThread = db.DiscussionThreads.Find(id);
            ViewBag.CurrentDiscussionThread = discussionThread;
            return View(discussionThread);
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult Edit(int id)
        {
            DiscussionThread discussionThread = db.DiscussionThreads.Find(id);
            ViewBag.DiscussionThread = discussionThread;
            if (discussionThread.UserId.Equals(User.Identity.GetUserId()) || User.IsInRole("Admin, Moderator"))
            {
                return View(discussionThread);
            }
            else
            {
                TempData["message"] = "Daca nu e firul de discutie initiat de tine, de ce vrei sa-ti bagi nasul sa-l stergi?";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = discussionThread.SubjectId}));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult Edit(int id, DiscussionThread requestDiscussionThread)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DiscussionThread discussionThread = db.DiscussionThreads.Find(id);
                    if (discussionThread.UserId.Equals(User.Identity.GetUserId()) || User.IsInRole("Admin, Moderator"))
                    {
                        if (TryUpdateModel(discussionThread))
                        {
                            discussionThread.Name = requestDiscussionThread.Name;
                            discussionThread.SubjectId = requestDiscussionThread.SubjectId;
                            discussionThread.Description = requestDiscussionThread.Description;
                            discussionThread.Date = DateTime.Now;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = discussionThread.SubjectId }));
                    }
                    else
                    {
                        TempData["message"] = "Huh, nu e firul de discutie initiat de tine, nu umbla.";
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = discussionThread.SubjectId}));
                    }
                }
                else
                {
                    return View(requestDiscussionThread);
                }
            }
            catch (Exception e)
            {
                return View(requestDiscussionThread);
            }
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult New(int subjectId)
        {
            DiscussionThread discussionThread = new DiscussionThread();
            discussionThread.SubjectId = subjectId;
            discussionThread.UserId = User.Identity.GetUserId();
            discussionThread.Date = DateTime.Now;
            return View(discussionThread);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult New(int subjectId, DiscussionThread discussionThread)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DiscussionThreads.Add(discussionThread);
                    db.SaveChanges();
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = discussionThread.SubjectId }));
                }
                else
                {
                    return View(discussionThread);
                }
            }
            catch (Exception e)
            {
                return View(discussionThread);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult Delete(int id)
        {
            DiscussionThread discussionThread = db.DiscussionThreads.Find(id);
            if (discussionThread.UserId.Equals(User.Identity.GetUserId()) || User.IsInRole("Admin, Moderator"))
            {
                db.DiscussionThreads.Remove(discussionThread);
                db.SaveChanges();
                TempData["message"] = "Firul de discutie si-a schimbat culoarea.";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = discussionThread.SubjectId }));
            }
            else
            {
                TempData["message"] = "Iarasi, devii insistent, nu mai incerca sa modifici ce nu-ti apartine...";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = discussionThread.SubjectId}));
            }
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllSubjects()
        {
            var selectList = new List<SelectListItem>();
            var subjects = from subject in db.Subjects select subject;
            foreach (var subject in subjects)
            {
                selectList.Add(new SelectListItem
                {
                    Value = subject.Id.ToString(),
                    Text = subject.Name.ToString()
                });
            }

            return selectList;
        }
    }
}