using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Details
        public ActionResult Index(int id)
        {
            using (var db = new trangsucbacEntities())
            {
                var product = db.SanPhams.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                var danhMuc = db.DanhMucs.FirstOrDefault(dm => dm.idDanhMuc == product.idDanhMuc);
                var sanPhamLienQuan = db.SanPhams
                    .Where(sp => sp.idDanhMuc == product.idDanhMuc && sp.idSanPham != id)
                    .ToList();
                var listDanhMuc = db.DanhMucs.ToList();
                ViewBag.TenDanhMuc = danhMuc?.tenDanhMuc;
                ViewBag.SanPhamLienQuan = sanPhamLienQuan;
                ViewBag.ListDanhMuc = listDanhMuc;
                return View(product);
            }
        }
    }
}