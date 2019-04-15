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
    public class HocPhanController : Controller
    {
        private HocPhanModel model = new HocPhanModel();
        // GET: HocPhan
        public ActionResult Index()
        {
            return View(model.findAll());
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            ViewBag.giangviens = new GiangVienModel().findAll();
            ViewBag.lophocs = new LopHocModel().findAll();
            return View("Create", new HocPhan());
        }
        [HttpPost]
        public ActionResult Create(HocPhan HocPhan)
        {
            
            model.create(HocPhan);
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
            ViewBag.giangviens = new GiangVienModel().findAll();
            ViewBag.lophocs = new LopHocModel().findAll();

            ViewBag.id = id;
            HocPhan hocPhan = model.find(id);
            return View("Edit", model.find(id));
        }
        [HttpPost]
        public ActionResult Edit(HocPhan objectname, string id)
        {
            objectname.Id = ObjectId.Parse(id);
            model.update(objectname);
            return RedirectToAction("Index");
        }

    }
}