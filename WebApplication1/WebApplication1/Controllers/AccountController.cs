using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private AccountModel accountModel = new AccountModel();
        // GET: Account
        public ActionResult Index()
        {
            ViewBag.accounts = accountModel.findAll();
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View("Add", new Account());
        }
        [HttpPost]
        public ActionResult Add(Account account)
        {
            accountModel.create(account);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            accountModel.delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.id = id;
            return View("Edit", accountModel.find(id));
        }
        [HttpPost]
        public ActionResult Edit(Account account,String AccountId)
        {
            var currentaccount = accountModel.find(AccountId);
            currentaccount.Password = account.Password;
            currentaccount.Fullname = account.Fullname;
            currentaccount.Username = account.Username;

            accountModel.update(currentaccount);
            return RedirectToAction("Index");
        }

    }
}