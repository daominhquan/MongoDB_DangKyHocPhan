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
            if (account.Username == "admin" && account.Password == "admin")
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

            ViewBag.lophocs = new LopHocModel().findAll();
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(Account account, string retypePassword)
        {
            ViewBag.lophocs = new LopHocModel().findAll();
            if (account.Fullname == null)
            {
                ModelState.AddModelError("Fullname", "không được để trống");
                return View();
            }
            if (account.id_LopHoc == null)
            {
                ModelState.AddModelError("id_LopHoc", "không được để trống");
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
        [HttpGet]
        public ActionResult DangKyHocPhan()
        {
            if (Session[currentAccount] == null)
            {
                return RedirectToAction("Index");
            }

            MonHocModel monHoc = new MonHocModel();
            ViewBag.listMonHoc = monHoc.findAll();
            ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());

            return View();
        }
        [HttpPost]
        public ActionResult DangKyHocPhan(List<string> DanhSachHocPhan)
        {
            if (!isUserNameExist())
            {
                return RedirectToAction("Index");
            }
            foreach (string item in DanhSachHocPhan)
            {
                if (item == null)
                {
                    DanhSachHocPhan.Remove(item);
                }
            }

            MonHocModel monHoc = new MonHocModel();
            HocPhanModel hocPhanModel = new HocPhanModel();
            AccountModel accountModel = new AccountModel();
            Account account = new AccountModel().find_username(Session[currentAccount].ToString());
            List<string> newHocPhanDaDangKy = DanhSachHocPhan;
            account.HocPhanDaDangKy = newHocPhanDaDangKy;
            if (account != null)
            {
                accountModel.update(account);
            }

            ViewBag.listMonHoc = monHoc.findAll();
            ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());

            TempData["script"] = "toastr.success('Đăng ký học phần thành công!', 'Thành công')";
            return View("Index");
        }
        public bool isUserNameExist()
        {
            AccountModel accountModel = new AccountModel();
            foreach (var item in accountModel.findAll())
            {
                if (Session[currentAccount].ToString() == item.Username)
                {
                    return true;
                }
            }
            return false;
        }
    }
}