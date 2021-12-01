using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using MyPham.Models;

namespace MyPham.Controllers
{
    public class TaiKhoanController : Controller
    {
        MyPhamDB db = new MyPhamDB();

        // GET: TaiKhoan
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
                var user = db.TaiKhoan.Where(u => u.Email.Equals(email) && u.MatKhau.Equals(matkhau)).ToList();
                if (user.Count() > 0)
                {
                    //add session
                    Session["Email"] = user.FirstOrDefault().Email;
                    Session["HoTen"] = user.FirstOrDefault().HoTen;
                    Session["Anh"] = user.FirstOrDefault().Anh;
                    Session["idUser"] = user.FirstOrDefault().MaTK;
                    int MaTK = (int)Session["idUser"];
                    GioHang gh = (GioHang)checkGH(MaTK);
                    if (gh == null)
                    {
                        gh = new GioHang();
                        gh.MaTK = MaTK;
                        db.GioHang.Add(gh);
                        db.SaveChanges();
                        GioHang gio = (GioHang)checkGH(MaTK);
                        int MaGH = gio.MaGioHang;
                        Session["MaGH"] = MaGH;
                    }
                    else
                    {
                        List<Chi_Tiet_Gio_Hang> chi_Tiet_Gio_Hang = db.Chi_Tiet_Gio_Hang.Where(g => g.MaGioHang == gh.MaGioHang).ToList();
                        if(chi_Tiet_Gio_Hang.Count >0)
                        {
                            List<Gio> list = new List<Gio>();
                            foreach (var item in chi_Tiet_Gio_Hang)
                            {
                                SanPham x = db.SanPham.Find(item.MaSP);
                                Gio gio = new Gio();
                                gio.soLuong = item.SoLuongMua;
                                gio.sanPham = x;
                                
                                list.Add(gio);
                            }
                            Session["GioHang"] = list;
                            Session["SoLuong"] = list.Count;
                        }
                        else
                        {
                            Session["SoLuong"] = null;
                        }
                        Session["MaGH"] = gh.MaGioHang;
                    }    
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Đăng nhập không thành công!";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "DangKyCaptcha", "Mã xác nhận không đúng !")]
        public ActionResult DangKy(TaiKhoan dangky)
        {
            if (ModelState.IsValid)
            {
                if (checkEmail(dangky.Email))
                {
                    ViewBag.error = "Email đã tồn tại";
                }
                else
                {
                    var user = new TaiKhoan();
                    user.Email = dangky.Email;
                    user.MatKhau = dangky.MatKhau;
                    user.HoTen = dangky.HoTen;
                    user.DiaChi = dangky.DiaChi;
                    user.SoDienThoai = dangky.SoDienThoai;
                    user.TinhTrang = true;
                    user.MaQuyen = 3;
                    db.TaiKhoan.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return View(dangky);
        }

        private bool checkEmail(string email)
        {
            return db.TaiKhoan.Count(t => t.Email == email) > 0;
        }

        public ActionResult ThongTinTaiKhoan(int? id)
        {
            
            if (id == null || Session["idUser"] == null)
            {

                return RedirectToAction("Index", "Home");

            }
            if((int)Session["idUser"] != id)
            {
                return RedirectToAction("Index", "Home");
            }
            TaiKhoan nguoidung = db.TaiKhoan.Find(id);
            Session["Anh"] = nguoidung.Anh;
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            return View(nguoidung);
        }

        public ActionResult SuaTaiKhoan(int? id)
        {
            if (id == null || Session["idUser"] == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index", "Home");
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTaiKhoan([Bind(Include = "MaTK,Email,MatKhau,HoTen,DiaChi,SoDienThoai,Anh,TinhTrang,MaQuyen")] TaiKhoan taiKhoan )
        {
            if (ModelState.IsValid)
            {
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/wwwroot/images/user/" + FileName);
                    f.SaveAs(UploadPath);
                    taiKhoan.Anh = FileName;
                }
                else
                {
                    string UploadPath = Server.MapPath("~/wwwroot/image/" + taiKhoan.Anh);
                }
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThongTinTaiKhoan","TaiKhoan",new { id= taiKhoan.MaTK });
            }
            ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }
        private GioHang checkGH(int MaTK)
        {
            var GH = db.GioHang.Where(s => s.MaTK == MaTK);
            foreach (var item in GH)
            {
                var listDH = db.HoaDon.Where(h => h.MaGioHang == item.MaGioHang).FirstOrDefault();
                if (listDH == null)
                {
                    return item;
                }
            }
            return null;
        }
    }
}