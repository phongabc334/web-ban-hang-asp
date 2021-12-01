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
    public class SanPhamsController : BaseController
    {
        private MyPhamDB db = new MyPhamDB();

        // GET: Admin/SanPhams
        public ActionResult Index(int? page,string error,string maDM)
        {

            if (error != null)
                ViewBag.Error = error;
            var sanPham = db.SanPham.Include(s => s.DanhMucSP);
            ViewBag.DanhMuc = db.DanhMucSP.ToList();
            if(maDM != null && maDM!= "macdinh")
            {
                sanPham = sanPham.Where(s=>s.MaDM.ToString().Equals(maDM));
                ViewBag.MaDM = maDM;
            }
            sanPham = sanPham.OrderBy(s => s.MaSP);
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(sanPham.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM");
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,Gia,ThuongHieu,SoLuongTon,XuatXu,DungTich,HangSX,TrongLuong,ChatLieu,CongDung,ThanhPhan,HuongDanSD,QuyCachDongGoi,GiamGia,MaDM,AnhSP")] SanPham sanPham)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    sanPham.AnhSP = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/images/SanPham/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    db.SanPham.Add(sanPham);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM", sanPham.MaDM);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM", sanPham.MaDM);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,Gia,ThuongHieu,SoLuongTon,XuatXu,DungTich,HangSX,TrongLuong,ChatLieu,CongDung,ThanhPhan,HuongDanSD,QuyCachDongGoi,GiamGia,MaDM,AnhSP")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwroot/images/SanPham/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    //else
                    //{
                    //    string UploadPath = Server.MapPath("~/wwwroot/image/" + sanPham.AnhSP);
                    //}
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSP, "MaDM", "TenDM", sanPham.MaDM);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult DeleteConfirmedCustom(int id)
        {
            SanPham sanPham = db.SanPham.Find(id);
      
            try
            {
                db.SanPham.Remove(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                return RedirectToAction("Index", "SanPhams", new { error = "Không  xóa  được  bản  ghi  này! " });
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
