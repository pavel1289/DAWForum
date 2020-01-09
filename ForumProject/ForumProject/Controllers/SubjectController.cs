using ForumProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ForumProject.Controllers
{
    public class SubjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int id)
        {
            var subjects = from subject in db.Subjects
                           orderby subject.Name
                           where subject.Category.Id == id
                           select subject;
            ViewBag.Subjects = subjects;
            ViewBag.CurrentCategory = db.Categories.Find(id);
            return View();
        }

        public ActionResult Show(int subjectId)
        {
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "DiscussionThread", action = "Index", id = subjectId }));
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit(int id)
        {
            Subject subject = db.Subjects.Find(id);
            ViewBag.Subject = subject;
            subject.Categories = GetAllCategories();
            return View(subject);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit(int id, Subject requestSubject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Subject subject = db.Subjects.Find(id);
                    if (TryUpdateModel(subject))
                    {
                        subject.Name = requestSubject.Name;
                        subject.CategoryId = requestSubject.CategoryId;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Subject", action = "Index", id = subject.CategoryId}));
                }
                else
                {
                    return View(requestSubject);
                }
            } catch (Exception e)
            {
                return View(requestSubject);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult New(int categoryId)
        {
            Subject subject = new Subject();
            subject.CategoryId = categoryId;
            return View(subject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult New(int categoryId, Subject subject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Subjects.Add(subject);
                    db.SaveChanges();
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Subject", action = "Index", id = subject.CategoryId}));
                }
                else
                {
                    return View(subject);
                }
            } catch (Exception e)
            {
                return View(subject);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Delete(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Subject", action = "Index", id = subject.CategoryId}));
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = from category in db.Categories select category;
            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }

            return selectList;
        }
    }
}