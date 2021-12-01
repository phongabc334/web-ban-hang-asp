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

namespace MyPham.Controllers
{
    public class SanPhamController : Controller
    {
        MyPhamDB db = new MyPhamDB();
        // GET: SanPham
        public ActionResult SanPham(string id, string ThongBao)
        {
            if (ThongBao != null)
            {
                ViewBag.ThongBao = ThongBao;
            }
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

        public ActionResult XemSanPhamTheoDanhMuc(string id, int? page, string loc)
        {

            var sanpham = db.SanPham.Where(s => s.MaDM.ToString().Equals(id)).Select(s => s);

            if (loc != null)
            {
                if (loc.Equals("tang"))
                {
                    sanpham = sanpham.OrderBy(s =>s.GiamGia);
                    ViewBag.Loc = loc;
                }
                else if (loc.Equals("giam"))
                {
                    sanpham = sanpham.OrderByDescending(s =>s.GiamGia);
                    ViewBag.Loc = loc;
                }
            }
            int madm = int.Parse(id);
            List<DanhMucSP> s1 = new List<DanhMucSP>();
            s1 = db.DanhMucSP.Where(h => h.MaDM == madm).ToList();
            ViewBag.TenDM = s1[0].TenDM;

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(sanpham.ToList().ToPagedList(pageNumber, pageSize));
        }
    }
}