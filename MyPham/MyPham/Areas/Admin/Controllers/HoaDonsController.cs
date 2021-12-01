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
    public class HoaDonsController : BaseController
    {
        private MyPhamDB db = new MyPhamDB();

        // GET: Admin/HoaDons

        public ActionResult Index(int? page, string error, string tinhtrang)
        {

            if (error != null)
                ViewBag.Error = error;
            List<HoaDon> hoaDon= new List<HoaDon>();
            hoaDon  = db.HoaDon.Include(h => h.GioHang).ToList();
            if (tinhtrang != null && tinhtrang != "macdinh")
            {
                hoaDon = hoaDon.Where(h => h.TinhTrang == tinhtrang).ToList();
                ViewBag.tinhtrang = tinhtrang;
            }
            hoaDon = hoaDon.OrderBy(s => s.MaHD).ToList();
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(hoaDon.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDon.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            var chiTiet = db.Chi_Tiet_Gio_Hang.Where(s => s.MaGioHang == hoaDon.MaGioHang).ToList();
            List<SanPham> sanpham = new List<SanPham>();
            foreach (var item in chiTiet)
            {
                SanPham x = new SanPham();
                x.MaSP = item.MaSP;
                x.Gia = item.GiaSP;
                var sp = db.SanPham.Find(item.MaSP);
                x.AnhSP = sp.AnhSP;
                x.TenSP = sp.TenSP;
                x.SoLuongTon = item.SoLuongMua;
                sanpham.Add(x);
            }
            ViewBag.SanPham = sanpham;
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Create
        public ActionResult Create()
        {
            ViewBag.MaGioHang = new SelectList(db.GioHang, "MaGioHang", "MaGioHang");
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,NgayTao,HinhThucVanChuyen,HinhThucThanhToan,MaGioHang,TinhTrang")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.HoaDon.Add(hoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaGioHang = new SelectList(db.GioHang, "MaGioHang", "MaGioHang", hoaDon.MaGioHang);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDon.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGioHang = new SelectList(db.GioHang, "MaGioHang", "MaGioHang", hoaDon.MaGioHang);
            ViewBag.TinhTrang = hoaDon.TinhTrang;
            return View(hoaDon);
        }

        // POST: Admin/HoaDons1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,NgayTao,HinhThucVanChuyen,HinhThucThanhToan,MaGioHang,GhiChu,HoTen,DiaChi,SoDienThoai,TinhTrang")] HoaDon hoaDon)
        {
   
            if (ModelState.IsValid)
            {
                db.Entry(hoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaGioHang = new SelectList(db.GioHang, "MaGioHang", "MaGioHang", hoaDon.MaGioHang);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDon.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            HoaDon hoaDon = db.HoaDon.Find(id);
            try
            {
                db.HoaDon.Remove(hoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "HoaDons", new { error = "Không thể xóa bản ghi này !!! " });
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
