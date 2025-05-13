using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class HomeController : Controller
    {
		public ActionResult Index()
		{
			using (var db = new trangsucbacEntities())
			{
				// Lấy danh sách sản phẩm (ví dụ: sản phẩm bán chạy)
				var similarProducts = db.SanPhams.Take(4).ToList(); // Lấy 10 sản phẩm đầu tiên
				ViewBag.SimilarProducts = similarProducts;
			}

			return View();
		}

		public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}