using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            try
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
            catch (MongoConnectionException)
            {
                ThongBao_Error("kết nối MongoDB không được: ");
                return View(account);
            }
            catch (Exception e)
            {
                ThongBao_Error("Lỗi : " + e.Message);
                return View(account);
            }

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
            try
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
            catch (MongoConnectionException)
            {
                ThongBao_Error("kết nối MongoDB không được: ");
                return View(account);
            }
            catch (Exception e)
            {
                ThongBao_Error("Lỗi : " + e.Message);
                return View(account);
            }
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
        //[HttpPost]
        //public ActionResult DangKyHocPhan(List<string> DanhSachHocPhan, List<string> MonHoc)
        //{
        //    if (!isUserNameExist())
        //    {
        //        return RedirectToAction("Index");
        //    }



        //    MonHocModel monHocModel = new MonHocModel();
        //    HocPhanModel hocPhanModel = new HocPhanModel();
        //    AccountModel accountModel = new AccountModel();
        //    Account account = new AccountModel().find_username(Session[currentAccount].ToString());
        //    List<string> HocPhanDaDangKy_OLD = account.HocPhanDaDangKy;
        //    string monHoc_Success = "";
        //    string monHoc_ThatBai = "";
        //    bool isThatBai = false;

        //    List<bool> soMonTrungVoiTruocDo = new List<bool>();

        //    if (account != null)
        //    {


        //        //Khởi tạo kết nối dưới dạng client connection đến MongoDB database của chúng ta
        //        var client = new MongoClient(new configWEB().connectionstring);
        //        //Create a session object that is used when leveraging transactions
        //        using (var session = client.StartSession())
        //        {
        //            session.StartTransaction(new TransactionOptions(
        //            readConcern: ReadConcern.Snapshot,
        //            writeConcern: WriteConcern.WMajority));
        //            try
        //            {
        //                //lock account khi bắt đầu transaction
        //                account.Status = true;
        //                accountModel.update(account);



        //                account.HocPhanDaDangKy = DanhSachHocPhan;
        //                //mở lại account khi transaction hoàn tất
        //                account.Status = false;
        //                accountModel.updateHocPhanDaDangKy(account, session);
        //                for (int i = 0; i < DanhSachHocPhan.Count(); i++)
        //                {

        //                    //nếu học phần đã đăng ký 
        //                    if (DanhSachHocPhan[i] != "")
        //                    {

        //                        while (monHocModel.getHocphan(MonHoc[i], DanhSachHocPhan[i]).Status == false)
        //                        {
        //                            //nếu học phần bị khóa 
        //                            //lock học phần 
        //                            monHocModel.lockHocPhan(DanhSachHocPhan[i]);
        //                            int conLai = monHocModel.ConLai(DanhSachHocPhan[i], monHocModel.getHocphan(MonHoc[i], DanhSachHocPhan[i]).SiSo);
        //                            //kiểm tra môn học còn trống hay không
        //                            if (conLai > 0)
        //                            {
        //                                //nếu còn trống thì lưu tên lại để thống báo
        //                                monHoc_Success = monHoc_Success + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
        //                            }
        //                            else
        //                            {

        //                                //nếu chưa đăng ký trước đó thì sẽ thất bại 
        //                                //chỉ cần một trong những môn đã đăng ký thất bại thì sẽ rollback toàn bộ
        //                                monHoc_ThatBai = monHoc_ThatBai + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
        //                                isThatBai = true;

        //                            }
        //                        }
        //                    }
        //                }


        //                //dừng 10 giây
        //                //Task.WaitAll(Task.Delay(5000));


        //                //nếu thất bại
        //                if (isThatBai)
        //                {
        //                    //unlock account khi hủy transaction
        //                    account.Status = true;
        //                    //mở khóa (unlock) những học phần đã khóa
        //                    for (int i = 0; i < DanhSachHocPhan.Count(); i++)
        //                    {
        //                        if (DanhSachHocPhan[i] != "")
        //                        {
        //                            monHocModel.unlockHocPhan(DanhSachHocPhan[i]);
        //                        }
        //                    }
        //                    accountModel.update(account);
        //                    ThongBao_Error("học phần môn " + monHoc_ThatBai + " đã hết chỗ , vui lòng chọn học phần khác");

        //                    //từ chối transaction này
        //                    session.AbortTransactionAsync();
        //                    ViewBag.listMonHoc = monHocModel.findAll();
        //                    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //                    return View(HocPhanDaDangKy_OLD);
        //                }
        //                //nếu không thất bại (thành công)
        //                else
        //                {
        //                    //mở khóa (unlock) những học phần đã khóa
        //                    for (int i = 0; i < DanhSachHocPhan.Count(); i++)
        //                    {
        //                        if (DanhSachHocPhan[i] != "")
        //                        {
        //                            monHocModel.unlockHocPhan(DanhSachHocPhan[i]);
        //                        }
        //                    }
        //                    session.CommitTransaction();
        //                    ThongBao_Success("Đăng ký học phần " + monHoc_Success + "  thành công!");
        //                    ViewBag.listMonHoc = monHocModel.findAll();
        //                    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //                    return View(DanhSachHocPhan);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                //unlock account khi hủy transaction
        //                account.Status = true;
        //                accountModel.update(account);

        //                ThongBao_Error("Error writing to MongoDB: " + e.Message);
        //                session.AbortTransaction();
        //                ViewBag.listMonHoc = monHocModel.findAll();
        //                ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //                return View(HocPhanDaDangKy_OLD);
        //            }

        //        }

        //    }




        //    ViewBag.listMonHoc = monHocModel.findAll();
        //    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //    return View(DanhSachHocPhan);
        //}
        //[HttpPost]
        //public ActionResult DangKyHocPhan(List<string> DanhSachHocPhan, List<string> MonHoc)
        //{
        //    if (!isUserNameExist())
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    //Create client connection to our MongoDB database
        //    var client = new MongoClient(new configWEB().connectionstring);

        //    //Create a session object that is used when leveraging transactions


        //    MonHocModel monHocModel = new MonHocModel();
        //    HocPhanModel hocPhanModel = new HocPhanModel();
        //    AccountModel accountModel = new AccountModel();
        //    Account account = new AccountModel().find_username(Session[currentAccount].ToString());
        //    List<string> HocPhanDaDangKy_OLD = account.HocPhanDaDangKy;
        //    string monHoc_Success = "";
        //    string monHoc_ThatBai = "";
        //    bool isThatBai = false;
        //    if (account != null)
        //    {



        //        using (var session = client.StartSession())
        //        {
        //            session.StartTransaction(new TransactionOptions(
        //            readConcern: ReadConcern.Snapshot,
        //            writeConcern: WriteConcern.WMajority));
        //            try
        //            {
        //                //lock account khi bắt đầu transaction
        //                account.Status = true;
        //                accountModel.update(account);

        //                account.HocPhanDaDangKy = DanhSachHocPhan;
        //                accountModel.updateHocPhanDaDangKy(account, session);
        //                for (int i = 0; i < DanhSachHocPhan.Count(); i++)
        //                {
        //                    if (DanhSachHocPhan[i] != "")
        //                    {
        //                        while (monHocModel.getHocphan(MonHoc[i], DanhSachHocPhan[i]).Status == false)
        //                        {
        //                            //nếu học phần bị khóa lock học phần
        //                            monHocModel.lockHocPhan(DanhSachHocPhan[i]);
        //                            if (monHocModel.ConLai(account.HocPhanDaDangKy[i], monHocModel.getHocphan(MonHoc[i], account.HocPhanDaDangKy[i]).SiSo) > 0)
        //                            {
        //                                monHoc_Success = monHoc_Success + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
        //                            }
        //                            else
        //                            {
        //                                monHoc_ThatBai = monHoc_ThatBai + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
        //                                isThatBai = true;
        //                            }
        //                        }

        //                    }
        //                }


        //                //mở khóa (unlock) những học phần đã khóa
        //                for (int i = 0; i < DanhSachHocPhan.Count(); i++)
        //                {
        //                    if (DanhSachHocPhan[i] != ""){ monHocModel.unlockHocPhan(DanhSachHocPhan[i]);       }
        //                }
        //                //dừng 10 giây
        //                Task.WaitAll(Task.Delay(5000));
        //                if (isThatBai)
        //                {
        //                    ThongBao_Error("học phần môn " + monHoc_ThatBai + " đã hết chỗ , vui lòng chọn học phần khác");
        //                    session.AbortTransactionAsync();
        //                    ViewBag.listMonHoc = monHocModel.findAll();
        //                    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //                    return View(HocPhanDaDangKy_OLD);
        //                }
        //                else
        //                {



        //                    ThongBao_Success("Đăng ký học phần " + monHoc_Success + "  thành công!");
        //                    session.CommitTransaction();
        //                    ViewBag.listMonHoc = monHocModel.findAll();
        //                    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //                    return View(DanhSachHocPhan);
        //                }
        //            }
        //            catch (Exception e)
        //            {

        //                //unlock account khi hủy transaction
        //                account.Status = true;
        //                accountModel.update(account);

        //                ThongBao_Error("Error writing to MongoDB: " + e.Message);
        //                session.AbortTransaction();
        //                ViewBag.listMonHoc = monHocModel.findAll();
        //                ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //                return View(HocPhanDaDangKy_OLD);
        //            }
        //        }

        //    }




        //    ViewBag.listMonHoc = monHocModel.findAll();
        //    ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
        //    return View(DanhSachHocPhan);
        //}
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


            MonHocModel monHocModel = new MonHocModel();
            HocPhanModel hocPhanModel = new HocPhanModel();
            AccountModel accountModel = new AccountModel();
            Account account = new AccountModel().find_username(Session[currentAccount].ToString());

            List<string> HocPhanDaDangKy_OLD = account.HocPhanDaDangKy;
            List<bool> trungID = new List<bool>();
            for (int i = 0; i < HocPhanDaDangKy_OLD.Count(); i++)
            {
                if (HocPhanDaDangKy_OLD[i] == DanhSachHocPhan[i])
                {
                    trungID.Add(true);
                }
                else
                {
                    trungID.Add(false);
                }
            }
            string monHoc_Success = "";
            string monHoc_ThatBai = "";
            bool isThatBai = false;
            if (account != null)
            {



                using (var session = client.StartSession())
                {
                    account.Status = true;
                    accountModel.update(account);
                    session.StartTransaction(new TransactionOptions(
                    readConcern: ReadConcern.Snapshot,
                    writeConcern: WriteConcern.WMajority));
                    try
                    {
                        account.Status = false;
                        account.HocPhanDaDangKy = DanhSachHocPhan;
                        accountModel.updateHocPhanDaDangKy(account, session);
                        for (int i = 0; i < DanhSachHocPhan.Count(); i++)
                        {

                            if (DanhSachHocPhan[i] != "")
                            {
                                while (monHocModel.getHocphan(MonHoc[i], account.HocPhanDaDangKy[i]).Status == true)
                                {
                                    if (monHocModel.getHocphan(MonHoc[i], account.HocPhanDaDangKy[i]).Status == false)
                                        break;
                                }
                                monHocModel.lockHocPhan(DanhSachHocPhan[i]);

                                if (monHocModel.ConLai(account.HocPhanDaDangKy[i], 
                                                       monHocModel.getHocphan(MonHoc[i],
                                                       account.HocPhanDaDangKy[i]).SiSo) > 0 || trungID[i]==true)
                                {
                                    monHoc_Success = monHoc_Success + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
                                }
                                else
                                {
                                    monHoc_ThatBai = monHoc_ThatBai + ", " + monHocModel.find(MonHoc[i]).TenMonHoc + " ";
                                    isThatBai = true;
                                }
                            }

                        }



                        Thread.Sleep(3000);




                        for (int i = 0; i < DanhSachHocPhan.Count(); i++)
                        {
                            if (DanhSachHocPhan[i] != "")
                            {
                                monHocModel.unlockHocPhan(DanhSachHocPhan[i]);
                            }
                        }

                        if (isThatBai)
                        {
                            ThongBao_Error("học phần môn " + monHoc_ThatBai + " đã hết chỗ , vui lòng chọn học phần khác");
                            session.AbortTransactionAsync();
                            ViewBag.listMonHoc = monHocModel.findAll();
                            ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());

                            account.Status = false;
                            account.HocPhanDaDangKy = HocPhanDaDangKy_OLD;
                            accountModel.update(account);
                            return View(HocPhanDaDangKy_OLD);
                        }
                        else
                        {
                            ThongBao_Success("Đăng ký học phần " + monHoc_Success + "  thành công!");
                            session.CommitTransaction();
                            ViewBag.listMonHoc = monHocModel.findAll();
                            ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
                            return View(DanhSachHocPhan);
                        }
                    }
                    catch (Exception e)
                    {
                        account.Status = false;
                        account.HocPhanDaDangKy = HocPhanDaDangKy_OLD;
                        accountModel.update(account);

                        ThongBao_Error("Error writing to MongoDB: " + e.Message);
                        session.AbortTransaction();
                        ViewBag.listMonHoc = monHocModel.findAll();
                        ViewBag.accountInfo = accountModel.find_username(Session[currentAccount].ToString());
                        return View(HocPhanDaDangKy_OLD);
                    }
                }

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