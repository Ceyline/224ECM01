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
            int userId = 2; // Giả định userId là 2

            // Lấy danh sách sản phẩm trong giỏ hàng
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
