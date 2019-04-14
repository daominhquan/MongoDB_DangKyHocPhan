using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private const string currentAccount = "currentAccount";
        AccountModel accountModel = new AccountModel();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session[currentAccount] != null)
            {
                return View("Index");
            }
            return View(new Account());
        }

        [HttpPost]
        public ActionResult DangNhap(Account account)
        {
            if(account.Username=="admin" && account.Password == "admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (accountModel.find_username(account.Username) == null)
                {
                    ModelState.AddModelError("", "không tồn tại tài khoản này");
                    return View();
                }
                else
                {
                    if (
                        (accountModel.find_username(account.Username).Username == account.Username) &&
                        (accountModel.find_username(account.Username).Password == account.Password)
                       )
                    {
                        Session[currentAccount] = account.Username;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "tên đăng nhập hoặc mật khẩu không đúng");
                        return View();
                    }
                }
            }
            return View(account);

        }
        [HttpGet]
        public ActionResult DangKy()
        {
            if (Session[currentAccount] != null)
            {
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(Account account, string retypePassword)
        {
            if (account.Fullname == null)
            {
                ModelState.AddModelError("Fullname", "không được để trống");
                return View();
            }
            if (ModelState.IsValid)
            {
                if (accountModel.find_username(account.Username) != null)
                {
                    ModelState.AddModelError("", "tài khoản đã tồn tại");
                    return View();
                }
                else
                {
                    if (account.Password == retypePassword)
                    {
                        accountModel.create(account);
                        Session[currentAccount] = account.Username;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "[nhập lại mật khẩu] và [mật khẩu] không trùng khớp nhau");
                        return View();
                    }
                }
            }
            return View(account);
        }
        [HttpGet]
        public ActionResult DangXuat()
        {
            Session[currentAccount] = null;
            return RedirectToAction("Index");
        }

    }
}