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
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index(int id)
        {
            var comments = from comment in db.Comments
                           orderby comment.Date
                           where comment.DiscussionThreadId.Equals(id)
                           select comment;
            fillUsers(comments);
            ViewBag.Comments = comments;
            ViewBag.CurrentDiscussionThread = db.DiscussionThreads.Find(id);
            return View();
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            ViewBag.Comment = comment;
            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator, Admin"))
            {
                return View(comment);
            }
            else
            {
                TempData["message"] = "Nu umbla daca nu e comentariul tau.";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Comment", action = "Index", id = comment.DiscussionThreadId}));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment = db.Comments.Find(id);
                    if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator, Admin"))
                    {
                        if (TryUpdateModel(comment))
                        {
                            comment.Text = requestComment.Text;
                            comment.Date = requestComment.Date;
                            db.SaveChanges();
                            TempData["message"] = "Comentariu modificat :D";
                        }
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Comment", action = "Index", id = comment.DiscussionThreadId}));
                    }
                    else
                    {
                        TempData["message"] = "Nu umbla daca nu e comentariul tau.";
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Comment", action = "Index", id = comment.DiscussionThreadId}));
                    }
                }
                else
                {
                    return View(requestComment);
                }
            }
            catch (Exception e)
            {
                return View(requestComment);
            }
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult New(int discussionThreadId)
        {
            Comment comment = new Comment();
            comment.DiscussionThreadId = discussionThreadId;
            comment.UserId = User.Identity.GetUserId();
            comment.Date = DateTime.Now;
            return View(comment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult New(int discussionThreadId, Comment comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    TempData["message"] = "Comentariu adaugat.";
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Comment", action = "Index", id = comment.DiscussionThreadId}));
                }
                else
                {
                    return View(comment);
                }
            }
            catch (Exception e)
            {
                return View(comment);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Moderator, User")]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (User.IsInRole("Moderator, Admin") || comment.UserId.Equals(User.Identity.GetUserId()))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost sters pentru eternitate...o vreme foarte, foarte indelungata.";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Comment", action = "Index", id = comment.DiscussionThreadId}));
            }
            else
            {
                TempData["message"] = "Credeam ca m-am facut inteles, nu umbla daca nu e comentariul tau...";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Comment", action = "Index", id = comment.DiscussionThreadId}));
            }
        }

        [NonAction]
        private void fillUsers(IQueryable<Comment> comments)
        {
            var users = from user in db.Users
                        select user;
            var listUsers = users.ToList();
            foreach (var comment in comments)
            {
                var userId = comment.UserId;
                comment.User = listUsers.Find(x => x.Id.Equals(userId));
            }
        }
    }
}