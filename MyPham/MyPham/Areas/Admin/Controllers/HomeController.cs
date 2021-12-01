using MyPham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPham.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private MyPhamDB db = new MyPhamDB();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var doanhThu = db.Chi_Tiet_Gio_Hang.ToList();
            var hoadon = db.HoaDon.ToList();
            var taikhoan = db.TaiKhoan.Where(s => s.MaQuyen == 3).ToList();
            var sanpham = db.SanPham.ToList();

            double doanht = 0;
            int slton = 0;
            int slDaBan = 0;
            foreach (var item in doanhThu)
            {
                slDaBan += item.SoLuongMua;
                doanht += item.SoLuongMua * Convert.ToDouble(item.GiaSP);
            }
            foreach (var item in sanpham)
            {
                slton += item.SoLuongTon;
            }
            ViewBag.DoanhThu = doanht;
            ViewBag.TongSP = (slton + slDaBan);
            ViewBag.SLBan = slDaBan;
            ViewBag.SLKhach = taikhoan.Count;
            return View();
        }
       public ActionResult QuanLyPhanQuyen()
        {
            return View();
        }
        public ActionResult CauHinhQuyen()
        {
            return View();
        }
        public ActionResult ThemPhanQuyen()
        {
            return View();
        }
    }
}