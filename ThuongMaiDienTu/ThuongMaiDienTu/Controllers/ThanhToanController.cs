using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
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
    }
}
