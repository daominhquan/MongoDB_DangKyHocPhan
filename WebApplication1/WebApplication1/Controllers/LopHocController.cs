using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LopHocController : Controller
    {
        private LopHocModel model = new LopHocModel();
        // GET: LopHoc
        public ActionResult Index()
        {
            return View(model.findAll());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new LopHoc());
        }
        [HttpPost]
        public ActionResult Create(LopHoc objectt)
        {
            if (ModelState.IsValid)
            {
                model.create(objectt);
                return RedirectToAction("Index");
            }
            return View(objectt);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.find(id) == null)
            {
                return RedirectToAction("Index");
            }
            model.delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.find(id) == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.id = id;
            return View("Edit", model.find(id));
        }
        [HttpPost]
        public ActionResult Edit(LopHoc objectname, string id)
        {
            objectname.Id = ObjectId.Parse(id);
            model.update(objectname);
            return RedirectToAction("Index");
        }

    }
}