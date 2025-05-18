using System.Linq;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly trangsucbacEntities _context;

        public DanhMucController()
        {
            _context = new trangsucbacEntities();
        }

        // Phương thức Index (Hiển thị danh mục)
        public ActionResult Index(int? idDanhMuc)
        {
            var danhMucs = _context.DanhMucs.ToList();
            var sanPhams = _context.SanPhams.AsQueryable();

            if (idDanhMuc.HasValue)
            {
                sanPhams = sanPhams.Where(sp => sp.idDanhMuc == idDanhMuc);
            }

            var viewModel = new SanPhamViewModel
            {
                DanhMucs = danhMucs,
                SanPhams = sanPhams.ToList()
            };

            return View(viewModel);
        }
		[ChildActionOnly]
		public ActionResult MenuDanhMuc()
		{
			var list = _context.DanhMucs.ToList();
			return PartialView("_MenuDanhMuc", list);
		}
	}
}
