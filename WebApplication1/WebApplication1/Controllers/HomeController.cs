using MongoDB.Driver;
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
            if (!isUserNameExist())
            {
                return RedirectToAction("Index");
            }
            MonHocModel monHoc = new MonHocModel();
            ViewBag.listMonHoc = monHoc.findAll();
            Account account = accountModel.find_username(Session[currentAccount].ToString());
            if (account.HocPhanDaDangKy != null)
            {
                return View(account.HocPhanDaDangKy);
            }


            return View();
        }
        [HttpPost]
        public ActionResult DangKyHocPhan(List<string> DanhSachHocPhan, List<string> MonHoc)
        {
            if (!isUserNameExist())
            {
                return RedirectToAction("Index");
            }
            //Create client connection to our MongoDB database
            var client = new MongoClient(new configWEB().connectionstring);

            //Create a session object that is used when leveraging transactions
            var session = client.StartSession();

            MonHocModel monHocModel = new MonHocModel();
            HocPhanModel hocPhanModel = new HocPhanModel();
            AccountModel accountModel = new AccountModel();
            Account account = new AccountModel().find_username(Session[currentAccount].ToString());

            string monHoc_Success = "";
            string monHoc_ThatBai = "";
            bool isThatBai = false;
            if (account != null)
            {
                session.StartTransaction();
                account.HocPhanDaDangKy = DanhSachHocPhan;
                accountModel.update(account);


                ThongBao_Error("Error writing to MongoDB: ");
                session.AbortTransaction();
                ViewBag.listMonHoc = monHocModel.findAll();
                ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
                return View(DanhSachHocPhan);
                //try
                //{
                //    for (int i = 0; i < DanhSachHocPhan.Count(); i++)
                //    {
                //        if (DanhSachHocPhan[i] != "")
                //        {
                //            if (monHocModel.ConLai(account.HocPhanDaDangKy[i], monHocModel.getHocphan(MonHoc[i], account.HocPhanDaDangKy[i]).SiSo) >= 0)
                //            {
                //                monHoc_Success = monHoc_Success + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
                //            }
                //            else
                //            {
                //                monHoc_ThatBai = monHoc_ThatBai + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
                //                isThatBai = true;
                //            }
                //        }
                //    }
                //    if (isThatBai)
                //    {
                //        ThongBao_Error("học phần môn " + monHoc_ThatBai + " đã hết chỗ , vui lòng chọn học phần khác");
                //        session.AbortTransactionAsync();
                //    }
                //    else
                //    {
                //        ThongBao_Success("Đăng ký học phần " + monHoc_Success + "  thành công!");
                //        session.CommitTransaction();
                //    }
                //    ViewBag.listMonHoc = monHocModel.findAll();
                //    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
                //    return View(DanhSachHocPhan);

                //}
                //catch (Exception e)
                //{
                //    ThongBao_Error("Error writing to MongoDB: " + e.Message);
                //    session.AbortTransaction();
                //    ViewBag.listMonHoc = monHocModel.findAll();
                //    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
                //    return View(DanhSachHocPhan);
                //}
            }




            ViewBag.listMonHoc = monHocModel.findAll();
            ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
            return View(DanhSachHocPhan);
        }

        public void ThongBao_Success(string noiDung)
        {
            TempData["script"] = "toastr.success('" + noiDung + "', 'Thành công')";
        }
        public void ThongBao_Error(string noiDung)
        {
            TempData["script"] = "toastr.error('" + noiDung + "', 'Thất bại')";
        }
        public void ThongBao_Warning(string noiDung)
        {
            TempData["script"] = "toastr.warning('" + noiDung + "', 'Thất bại')";
        }

        public bool isUserNameExist()
        {
            if (Session[currentAccount] == null)
            {
                return false;
            }
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