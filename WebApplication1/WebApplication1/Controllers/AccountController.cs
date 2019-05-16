using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private AccountModel model = new AccountModel();
        // GET: Account
        public ActionResult Index()
        {
            return View(model.findAll());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new Account());
        }
        [HttpPost]
        public ActionResult Create(Account account)
        {
            model.create(account);
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
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            else if(model.find(id)==null)
            {
                return RedirectToAction("Index");
            }
            Account a = model.find(id);
            while (a.Status == true)
            {
                a = model.find(id);
                if(a.Status == false)
                {
                    break;
                }
            }
            ViewBag.id = id;
            return View("Edit", model.find(id));
        }
        [HttpPost]
        public ActionResult Edit(Account objectname, string id)
        {
            objectname.Id = ObjectId.Parse(id);
            model.update(objectname);
            return RedirectToAction("Index");
        }

    }
}