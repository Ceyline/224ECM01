using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
	public class LichSuDatHangController : Controller
	{
		private readonly trangsucbacEntities _context;

		// Constructor to initialize the database context
		public LichSuDatHangController()
		{
			_context = new trangsucbacEntities();
		}

		// GET: LichSuDatHang
		public ActionResult Index()
		{
			if (Session["idNguoiDung"] == null)
			{
				ViewBag.isLogin = false;
				return RedirectToAction("Index", "Account"); // Redirect to login if not logged in
			}

			ViewBag.isLogin = true;

			int userId = (int)Session["idNguoiDung"];
			System.Diagnostics.Debug.WriteLine("userId from session = " + userId); // ✅ THÊM DÒNG NÀY


			// Retrieve order history for the logged-in user
			var orderHistory = _context.HoaDons
	.Include(h => h.ChiTietHoaDons.Select(ct => ct.SanPham.DanhMuc))
	.Where(h => h.idNguoiDung == userId)
	.OrderByDescending(h => h.ngayLap)
	.ToList();

			// Debugging: Log the retrieved data
			System.Diagnostics.Debug.WriteLine("Order History Count: " + orderHistory.Count);

			return View(orderHistory); // Pass the order history to the view
		}

		// Dispose the context to release resources
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_context.Dispose();
			}
			base.Dispose(disposing);
		}
		public ActionResult Details(int id)
		{
			if (Session["idNguoiDung"] == null)
				return RedirectToAction("Index", "Account");

			int userId = (int)Session["idNguoiDung"];

			var hoaDon = _context.HoaDons
				.Include(h => h.ChiTietHoaDons.Select(ct => ct.SanPham.DanhMuc))
				.FirstOrDefault(h => h.idHoaDon == id && h.idNguoiDung == userId);

			if (hoaDon == null)
				return HttpNotFound("Hóa đơn không tồn tại hoặc bạn không có quyền xem.");

			return View("Details", hoaDon);
		}

	}
}