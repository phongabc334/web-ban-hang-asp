using MyPham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace MyPham.Controllers
{
    public class HomeController : Controller
    {
        MyPhamDB db = new MyPhamDB();
        public ActionResult Index()
        {
            List<SanPham> SpSon = new List<SanPham>();
            SpSon = db.SanPham.Where(h => h.MaDM == 4).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            List<SanPham> SpDuongDa = new List<SanPham>();
            SpDuongDa = db.SanPham.Where(h => h.MaDM == 1).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            List<SanPham> SpTrangDiem = new List<SanPham>();
            SpTrangDiem = db.SanPham.Where(h => h.MaDM == 2).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            List<DanhMucSP> danhmucsp = new List<DanhMucSP>();
            danhmucsp = db.DanhMucSP.ToList();
            ViewBag.danhmuc = danhmucsp;
            ViewBag.TrangDiem = SpTrangDiem;
            ViewBag.Son = SpSon;
            ViewBag.DuongDa = SpDuongDa;

                
            return View();
        }
        public ActionResult TimKiem(string strSearch, int? page)
        {
            var sanpham = db.SanPham.ToList();
            if (strSearch != null)
            {
                sanpham = db.SanPham.Where(s => s.TenSP.ToUpper().Trim().Contains(strSearch.ToUpper().Trim())).ToList();
            }
            ViewBag.strSearch = strSearch;
            if (sanpham.Count > 0)
            {
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View(sanpham.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Thongbao = "Không có sản phẩm nào";
                return View();
            }
        }
        public PartialViewResult _Nav()
        {
            var danhmuc = db.DanhMucSP.Select(d => d);
            return PartialView(danhmuc);
        }
        
        
        public ActionResult SanPham(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPham.Find(int.Parse(id));
            if (sp == null)
            {
                return HttpNotFound();
            }
            int madm = db.SanPham.Find(int.Parse(id)).MaDM;
            ViewBag.ma = madm;

            List<SanPham> Sp = new List<SanPham>();
            Sp = db.SanPham.Where(h => h.MaDM.Equals(madm)).OrderByDescending(h => h.GiamGia).Take(8).ToList();
            ViewBag.sp = Sp;


            List<DanhMucSP> s = new List<DanhMucSP>();
            s = db.DanhMucSP.Where(h => h.MaDM == madm).ToList();
            ViewBag.TenDM = s[0].TenDM;

            return View(sp);
        }

        public ActionResult XemSanPhamTheoDanhMuc( string id )
        {
            List<SanPham> sanpham = new List<SanPham>();
            if(id == null)
            {
               
            } else
            {
                sanpham = db.SanPham.Where(s => s.MaDM.ToString().Equals(id)).Select(s => s).ToList();

            }
            int madm = int.Parse(id);
            List<DanhMucSP> s1 = new List<DanhMucSP>();
            s1 = db.DanhMucSP.Where(h => h.MaDM == madm).ToList();
            ViewBag.TenDM = s1[0].TenDM;

            return View(sanpham);
        }


    }
}