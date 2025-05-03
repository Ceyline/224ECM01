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
                ViewBag.TenDanhMuc = danhMuc?.tenDanhMuc;
                return View(product);
            }
        }
    }
}