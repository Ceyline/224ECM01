using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly trangsucbacEntities _context;

        // Constructor
        public ThanhToanController()
        {
            _context = new trangsucbacEntities();
        }

        // Phương thức Index (Hiển thị giỏ hàng)
        public ActionResult Index()
        {
            if (Session["idNguoiDung"] == null)
            {
                ViewBag.isLogin = false;
            }
            else
            {
                ViewBag.isLogin = true;
            }
            int userId = (int)Session["idNguoiDung"];
            var gioHang = _context.GioHangs
                                  .Include(g => g.SanPham) // Kết hợp với bảng SanPham
                                  .Where(g => g.idNguoiDung == userId) // Lọc theo người dùng
                                  .ToList();

            // Tính tổng tiền của giỏ hàng
            var tongTien = gioHang.Sum(g => g.SanPham.GiaBan * g.SoLuong);

            // Truyền dữ liệu sang View
            ViewBag.TongTien = tongTien;
            return View(gioHang);
        }
		[HttpPost]
		public ActionResult DatHang(DatHangViewModel model)
		{
			if (ModelState.IsValid)
			{
				int userId = 2; // Hoặc lấy từ Session nếu có đăng nhập
				var gioHang = _context.GioHangs
									  .Include(g => g.SanPham)
									  .Where(g => g.idNguoiDung == userId)
									  .ToList();

				if (gioHang != null && gioHang.Any())
				{
					var hoaDon = new HoaDon
					{
						email = model.Email,
						hoTen = model.HoTen,
						soDienThoai = model.SoDienThoai,
						diaChi = model.DiaChi,
						ghiChu = model.GhiChu,
						ngayLap = DateTime.Now,
						tongTien = gioHang.Sum(x => x.SoLuong * x.SanPham.GiaBan),
						idNguoiDung = userId // Giả định user đang đăng nhập
					};

					_context.HoaDons.Add(hoaDon);
					_context.SaveChanges(); // lưu hóa đơn

					// TODO: Nếu có bảng ChiTietHoaDon thì thêm các dòng ở đây

					// Xóa giỏ hàng của user sau khi đặt

					_context.SaveChanges();

					return Json(new { success = true, message = "Đặt hàng thành công!" });
				}
				else
				{
					return Json(new { success = false, message = "Giỏ hàng trống." });
				}
			}

			return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
		}

	}
}
