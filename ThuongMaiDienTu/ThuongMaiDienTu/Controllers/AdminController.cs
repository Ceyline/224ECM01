using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;
using System.Data.Entity;

namespace ThuongMaiDienTu.Controllers
{
    public class AdminController : Controller
    {
        private trangsucbacEntities db = new trangsucbacEntities();

        // GET: Admin
        public ActionResult HomeAdmin()
        {
            return View();
        }

        public ActionResult QLUsers(int? id = null)
        {
            if (id.HasValue)
            {
                var user = db.NguoiDungs.Find(id.Value);
                if (user != null)
                {
                    db.NguoiDungs.Remove(user);
                    db.SaveChanges();
                }
            }

            var danhSachNguoiDung = db.NguoiDungs.ToList();
            return View(danhSachNguoiDung);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var user = db.NguoiDungs.Find(id);
            if (user != null)
            {
                db.NguoiDungs.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("QLUsers");
        }

        public ActionResult CEUsers(int? idNguoiDung, string chucnang)
        {
            ViewBag.ChucNang = chucnang;

            var model = new NguoiDung(); // Tạo model rỗng nếu là tạo mới
            if (chucnang == "edit" && idNguoiDung.HasValue)
            {
                model = db.NguoiDungs.Find(idNguoiDung.Value);
                if (model == null)
                {
                    return HttpNotFound();
                }
            }

            return View(model); // Trả về View chứa form
        }


        [HttpPost]
        public ActionResult EditUser(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLUsers");
            }
            ViewBag.ChucNang = "edit";
            return View("CEUsers", model);
        }

        [HttpPost]
        public ActionResult CreateUser(NguoiDung model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.NguoiDungs.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("QLUsers");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }

                ViewBag.ChucNang = "create";
                return View("CEUsers", model);
            }

            ViewBag.ChucNang = "create";
            return View("CEUsers", model);
        }

        public ActionResult ChiTietGD()
        {
            var hoaDons = db.HoaDons.Include("NguoiDung").ToList();
            return View(hoaDons);
        }

        // GET: /Admin/Admin/ThongKeSanPham
        public ActionResult ThongKeSanPham()
        {
            var thongKe = db.ChiTietHoaDons
                .GroupBy(ct => new
                {
                    ct.SanPham.TenSanPham,
                    ct.SanPham.DanhMuc.tenDanhMuc
                })
                .Select(g => new ThongKeSanPhamViewModel
                {
                    TenSanPham = g.Key.TenSanPham,
                    TenDanhMuc = g.Key.tenDanhMuc,
                    SoLuongBan = g.Sum(x => x.soLuong),
                    DoanhThu = g.Sum(x => x.soLuong * x.giaBan)
                })
                .OrderByDescending(x => x.DoanhThu)
                .ToList();

            return View(thongKe);
        }

        public ActionResult ThongKeDonHang(DateTime? tuNgay, DateTime? denNgay, string loaiThongKe = "ngay")
        {
            // Logic lấy dữ liệu như đã xử lý trước đó
            var thongKe = LayDuLieuThongKe(tuNgay, denNgay, loaiThongKe);

            var viewModel = new ThongKeDonHangViewModel
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                LoaiThongKe = loaiThongKe,
                DanhSachThongKe = thongKe
            };

            return View(viewModel);
        }

        //[HttpGet]
        //public ActionResult ExportExcel(DateTime? tuNgay, DateTime? denNgay, string loaiThongKe = "ngay")
        //{
        //    var thongKe = LayDuLieuThongKe(tuNgay, denNgay, loaiThongKe);

        //    var package = new ExcelPackage();
        //    var ws = package.Workbook.Worksheets.Add("ThongKeDonHang");

        //    ws.Cells["A1"].Value = "Thời gian";
        //    ws.Cells["B1"].Value = "Tổng đơn hàng";
        //    ws.Cells["C1"].Value = "Tổng doanh thu";

        //    for (int i = 0; i < thongKe.Count; i++)
        //    {
        //        var row = i + 2;
        //        var item = thongKe[i];

        //        string thoiGian = "";
        //        switch (loaiThongKe)
        //        {
        //            case "ngay":
        //                thoiGian = item.Ngay.HasValue
        //                    ? new DateTime(item.Nam, item.Thang, item.Ngay.Value).ToString("yyyy-MM-dd")
        //                    : "";
        //                break;
        //            case "thang":
        //                thoiGian = $"Tháng {item.Thang}/{item.Nam}";
        //                break;
        //            case "nam":
        //                thoiGian = $"Năm {item.Nam}";
        //                break;
        //        }

        //        ws.Cells[row, 1].Value = thoiGian;
        //        ws.Cells[row, 2].Value = item.SoLuongDonHang;
        //        ws.Cells[row, 3].Value = item.TongDoanhThu;
        //    }

        //    ws.Cells[ws.Dimension.Address].AutoFitColumns();

        //    var file = package.GetAsByteArray();
        //    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeDonHang.xlsx");
        //}



        //[HttpGet]
        //public ActionResult ExportPdf(DateTime? tuNgay, DateTime? denNgay, string loaiThongKe = "ngay")
        //{
        //    var thongKe = LayDuLieuThongKe(tuNgay, denNgay, loaiThongKe);

        //    var viewModel = new ThongKeDonHangViewModel
        //    {
        //        TuNgay = tuNgay,
        //        DenNgay = denNgay,
        //        LoaiThongKe = loaiThongKe,
        //        DanhSachThongKe = thongKe
        //    };

        //    return new ViewAsPdf("ThongKePdf", viewModel)
        //    {
        //        PageSize = Rotativa.AspNetCore.Options.Size.A4,
        //        FileName = "ThongKeDonHang.pdf"
        //    };
        //}

        private List<ThongKeDonHangItem> LayDuLieuThongKe(DateTime? tuNgay, DateTime? denNgay, string loaiThongKe)
        {
            var query = db.HoaDons.AsQueryable();

            if (tuNgay.HasValue)
                query = query.Where(h => h.ngayLap >= tuNgay.Value);
            if (denNgay.HasValue)
                query = query.Where(h => h.ngayLap <= denNgay.Value);

            var result = new List<ThongKeDonHangItem>();

            switch (loaiThongKe)
            {
                case "ngay":
                    result = query
                        .GroupBy(h => new
                        {
                            Day = h.ngayLap.Value.Day,
                            Month = h.ngayLap.Value.Month,
                            Year = h.ngayLap.Value.Year
                        })
                        .Select(g => new ThongKeDonHangItem
                        {
                            Ngay = g.Key.Day,
                            Thang = g.Key.Month,
                            Nam = g.Key.Year,
                            SoLuongDonHang = g.Count(),
                            TongDoanhThu = g.Sum(x => x.tongTien ?? 0)
                        })
                        .OrderBy(g => g.Nam).ThenBy(g => g.Thang).ThenBy(g => g.Ngay)
                        .ToList();
                    break;


                case "thang":
                    result = query
                        .GroupBy(h => new { h.ngayLap.Value.Year, h.ngayLap.Value.Month })
                        .Select(g => new ThongKeDonHangItem
                        {
                            Thang = g.Key.Month,
                            Nam = g.Key.Year,
                            SoLuongDonHang = g.Count(),
                            TongDoanhThu = g.Sum(x => x.tongTien ?? 0)
                        })
                        .OrderBy(g => g.Nam).ThenBy(g => g.Thang)
                        .ToList();
                    break;

                case "nam":
                    result = query
                        .GroupBy(h => h.ngayLap.Value.Year)
                        .Select(g => new ThongKeDonHangItem
                        {
                            Nam = g.Key,
                            SoLuongDonHang = g.Count(),
                            TongDoanhThu = g.Sum(x => x.tongTien ?? 0)
                        })
                        .OrderBy(g => g.Nam)
                        .ToList();
                    break;
            }
            return result; // Trả về dữ liệu thống kê phù hợp
        }
        // GET: Admin/QLSanPham
        public ActionResult QLSanPham(int? id = null)
        {
            if (id.HasValue)
            {
                var sanPham = db.SanPhams.Find(id.Value);
                if (sanPham != null)
                {
                    db.SanPhams.Remove(sanPham);
                    db.SaveChanges();
                }
            }

            var danhSachSanPham = db.SanPhams.Include(sp => sp.DanhMuc).ToList();
            return View(danhSachSanPham);
        }

        // POST: Admin/DeleteSanPham
        [HttpPost]
        public ActionResult DeleteSanPham(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham != null)
            {
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
            }
            return RedirectToAction("QLSanPham");
        }

        // GET: Admin/CESanPham
        public ActionResult CESanPham(int? idSanPham, string chucnang)
        {
            ViewBag.ChucNang = chucnang;
            ViewBag.DanhMucList = new SelectList(db.DanhMucs, "idDanhMuc", "tenDanhMuc");

            var model = new SanPham();
            if (chucnang == "edit" && idSanPham.HasValue)
            {
                model = db.SanPhams.Include(sp => sp.DanhMuc).FirstOrDefault(sp => sp.idSanPham == idSanPham.Value);
                if (model == null)
                {
                    return HttpNotFound();
                }
            }

            return View(model);
        }

        // POST: Admin/CreateSanPham
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSanPham(SanPham model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.SanPhams.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("QLSanPham");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }
            }

            ViewBag.ChucNang = "create";
            ViewBag.DanhMucList = new SelectList(db.DanhMucs, "idDanhMuc", "tenDanhMuc", model.idDanhMuc);
            return View("CESanPham", model);
        }

        // POST: Admin/EditSanPham
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSanPham(SanPham model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLSanPham");
            }

            ViewBag.ChucNang = "edit";
            ViewBag.DanhMucList = new SelectList(db.DanhMucs, "idDanhMuc", "tenDanhMuc", model.idDanhMuc);
            return View("CESanPham", model);
        }

    }
}