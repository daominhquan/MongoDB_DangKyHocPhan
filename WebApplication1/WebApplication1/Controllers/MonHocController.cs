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
    public class MonHocController : Controller
    {
        private MonHocModel model = new MonHocModel();
        // GET: MonHoc
        public ActionResult Index()
        {
            return View(model.findAll());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new MonHoc());
        }
        [HttpPost]
        public ActionResult Create(MonHoc objectt)
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
        public ActionResult Edit(MonHoc objectname, string id)
        {
            objectname.Id = ObjectId.Parse(id);
            model.update(objectname);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult XemDanhSachHocPhan(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.find(id) == null)
            {
                return RedirectToAction("Index");
            }
            MonHoc monHoc = model.find(id);
            List<HocPhan> listHocPhan = new List<HocPhan>();
            if (monHoc.DanhSachHocPhan != null)
            {
                listHocPhan = monHoc.DanhSachHocPhan.ToList();
            }
            ViewBag.id_monhoc = id;
            ViewBag.TenMonHoc = monHoc.TenMonHoc;
            return View(listHocPhan);
        }
        [HttpGet]
        public ActionResult ThemHocPhan(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.find(id) == null)
            {
                return RedirectToAction("Index");
            }
            MonHoc monHoc = model.find(id);
            ViewBag.id_monhoc = id;
            ViewBag.TenMonHoc = monHoc.TenMonHoc;
            ViewBag.giangviens = new GiangVienModel().findAll();
            ViewBag.lophocs = new LopHocModel().findAll();
            return View();
        }
        [HttpPost]
        public ActionResult ThemHocPhan(HocPhan objectname, string id_monhoc)
        {
            MonHoc monHoc = model.find(id_monhoc);
            if (monHoc.DanhSachHocPhan == null)
            {
                monHoc.DanhSachHocPhan = new List<HocPhan>();
            }
            monHoc.DanhSachHocPhan.Add(objectname);
            objectname.Id = ObjectId.GenerateNewId();
            model.update(monHoc);


            return RedirectToAction(id_monhoc, "MonHoc/XemDanhSachHocPhan");
        }
        [HttpGet]
        public ActionResult DeleteHocPhan(string Id_MonHoc, int position_hocphan)
        {
            if (Id_MonHoc == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.find(Id_MonHoc) == null)
            {
                return RedirectToAction("Index");
            }

            MonHoc MonHoc = model.find(Id_MonHoc);
            MonHoc.DanhSachHocPhan.RemoveAt(position_hocphan);
            model.update(MonHoc);


            List<HocPhan> listHocPhan = MonHoc.DanhSachHocPhan.ToList();
            ViewBag.id_monhoc = Id_MonHoc;
            ViewBag.TenMonHoc = MonHoc.TenMonHoc;
            return RedirectToAction(Id_MonHoc, "MonHoc/XemDanhSachHocPhan");
        }
        [HttpGet]
        public ActionResult EditHocPhan(string Id_MonHoc, int position_hocphan)
        {
            if (Id_MonHoc == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.find(Id_MonHoc) == null)
            {
                return RedirectToAction("Index");
            }
            


            MonHoc mon = model.find(Id_MonHoc);
            HocPhan hoc = mon.DanhSachHocPhan[position_hocphan];
            ViewBag.position_hocphan = position_hocphan;
            ViewBag.Id_MonHoc = Id_MonHoc;
            ViewBag.TenMonHoc = mon.TenMonHoc;
            ViewBag.giangviens = new GiangVienModel().findAll();
            ViewBag.lophocs = new LopHocModel().findAll();
            ViewBag.id_hocphan = hoc.Id;
            return View(hoc);
        }
        [HttpPost]
        public ActionResult EditHocPhan(HocPhan hocPhan, string Id_MonHoc, int position_hocphan,string id_hocphan)
        {
            MonHoc monHoc = model.find(Id_MonHoc);
            hocPhan.Id = ObjectId.Parse(id_hocphan);
            monHoc.DanhSachHocPhan[position_hocphan] = hocPhan;
            model.update(monHoc);
            return RedirectToAction(Id_MonHoc, "MonHoc/XemDanhSachHocPhan");
        }
    }
}