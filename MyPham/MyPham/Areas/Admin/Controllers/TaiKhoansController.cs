using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPham.Models;
using PagedList;

namespace MyPham.Areas.Admin.Controllers
{
    public class TaiKhoansController : BaseController
    {
        private MyPhamDB db = new MyPhamDB();

        // GET: Admin/TaiKhoans
        public ActionResult Index(int? page,string error)
        {
            if (error != null)
                ViewBag.Error = error;
            var taiKhoan = db.TaiKhoan.Include(t => t.PhanQuyen).ToList();
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(taiKhoan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/TaiKhoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Create
        public ActionResult Create()
        {
            ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen");
            return View();
        }

        // POST: Admin/TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,Email,MatKhau,HoTen,DiaChi,SoDienThoai,Anh,TinhTrang,MaQuyen")] TaiKhoan taiKhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    taiKhoan.Anh = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/images/user/" + FileName);
                        f.SaveAs(UploadPath);
                        taiKhoan.Anh = FileName;
                    }
                    db.TaiKhoan.Add(taiKhoan);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
                return View(taiKhoan);
            }

        }

        // GET: Admin/TaiKhoans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,Email,MatKhau,HoTen,DiaChi,SoDienThoai,Anh,TinhTrang,MaQuyen")] TaiKhoan taiKhoan)
        {
            try
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
                        string UploadPath = Server.MapPath("~/wwwroot/image/user/" + taiKhoan.Anh);
                    }
                    db.Entry(taiKhoan).State = EntityState.Modified;
                    db.SaveChanges();
                    if ((int)Session["idAdmin"] == taiKhoan.MaTK)
                    {
                        Session["AnhAdmin"] = taiKhoan.Anh;
                        Session["HoTenAdmin"] = taiKhoan.HoTen;
                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.MaQuyen = new SelectList(db.PhanQuyen, "MaQuyen", "TenQuyen", taiKhoan.MaQuyen);
                return View(taiKhoan);
            }
        }

        // GET: Admin/TaiKhoans/Delete/5
        public ActionResult Delete(int id)
        {
            GioHang giohang = db.GioHang.Where(g => g.MaTK == id).FirstOrDefault();
            var chiTietGioHang = db.Chi_Tiet_Gio_Hang.Where(c => c.MaGioHang == giohang.MaGioHang).ToList();
            foreach (var item in chiTietGioHang)
            {
                db.Chi_Tiet_Gio_Hang.Remove(item);
                db.SaveChanges();
            }
            var hoaDon = db.HoaDon.Where(c => c.MaGioHang == giohang.MaGioHang).ToList();
            foreach (var item in hoaDon)
            {
                db.HoaDon.Remove(item);
                db.SaveChanges();
            }
            db.GioHang.Remove(giohang);
            db.SaveChanges();
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            db.TaiKhoan.Remove(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            db.TaiKhoan.Remove(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteConfirmedCustom(int id)
        {

            //GioHang giohang = db.GioHang.Where(g => g.MaTK == id).FirstOrDefault();
            //if (giohang != null)
            //{
            //    var chiTietGioHang = db.Chi_Tiet_Gio_Hang.Where(c => c.MaGioHang == giohang.MaGioHang);
            //    if (chiTietGioHang != null)
            //        foreach (var item in chiTietGioHang)
            //        {
            //            db.Chi_Tiet_Gio_Hang.Remove(item);
            //            db.SaveChanges();
            //        }


            //    var hoaDon = db.HoaDon.Where(c => c.MaGioHang == giohang.MaGioHang);
            //    if (hoaDon != null)
            //        foreach (var item in hoaDon)
            //        {
            //            db.HoaDon.Remove(item);
            //            db.SaveChanges();
            //        }

            //    db.GioHang.Remove(giohang);
            //    db.SaveChanges();
            //}

            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (id == (int)Session["idAdmin"])
            {
                return RedirectToAction("Index", "TaiKhoans", new { error = "Không  xóa  được  tài khoản đang đăng nhập !" });
            }
            try
            {
                db.TaiKhoan.Remove(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                return RedirectToAction("Index", "TaiKhoans", new { error = "Không  được  Xóa  tài  khoản  này ! " });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
