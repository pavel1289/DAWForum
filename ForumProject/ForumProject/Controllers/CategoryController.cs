using ForumProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Category
        public ActionResult Index()
        {
            var categories = from category in db.Categories
                             orderby category.Name
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        public ActionResult Show(string id)
        {
            Category category = db.Categories.Find(id);

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                return View(category);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = db.Categories.Find(id);
                    if (TryUpdateModel(category))
                    {
                        category.Name = requestCategory.Name;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestCategory);
                }
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}