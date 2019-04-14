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
    public class GiangVienController : Controller
    {
        private GiangVienModel model = new GiangVienModel();
        // GET: GiangVien
        public ActionResult Index()
        {
            return View(model.findAll());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new GiangVien());
        }
        [HttpPost]
        public ActionResult Create(GiangVien GiangVien)
        {
            model.create(GiangVien);
            return RedirectToAction("Index");
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
        public ActionResult Edit(GiangVien objectname, string id)
        {
            objectname.Id = ObjectId.Parse(id);
            model.update(objectname);
            return RedirectToAction("Index");
        }

    }
}