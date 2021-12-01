using MyPham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPham.Areas.Admin.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: Admin/DangNhap
        private MyPhamDB db = new MyPhamDB();
        public ActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string email, string matkhau)
        {
            if (ModelState.IsValid)
            {
                var user = db.TaiKhoan.Where(u => u.Email.Equals(email) &&
                u.MatKhau.Equals(matkhau)).ToList();
                if (user.Count() > 0)
                {
                    Session["HoTenAdmin"] = user.FirstOrDefault().HoTen;
                    Session["EmailAdmin"] = user.FirstOrDefault().Email;
                    Session["AnhAdmin"] = user.FirstOrDefault().Anh;
                    Session["LoaiAdmin"] = user.FirstOrDefault().MaQuyen;
                    Session["idAdmin"] = user.FirstOrDefault().MaTK;
                    if (user.FirstOrDefault().MaQuyen == 3)
                    {
                        ModelState.AddModelError("", "Bạn Không Có Quyền Vào ADMIN!!");
                    }
                    else if (user.FirstOrDefault().TinhTrang == false)
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn đang bị khóa !!");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!!");
                }
            }

            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }
    }
}