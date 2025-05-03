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
            ThuongMaiDienTu.Models.trangsucbacEntities db = new ThuongMaiDienTu.Models.trangsucbacEntities();
            var product = db.SanPhams.Find(id);
            return View(product);
        }
    }
}