using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemGioHang(int idSanPham, int soLuong, string size)
        {
            try
            {
                using (var db = new trangsucbacEntities())
                {
                    

                    int idNguoiDung = 2; // Tạm thời hardcode

                    System.Diagnostics.Debug.WriteLine($"Params: idSanPham={idSanPham}, soLuong={soLuong}, size={size}, idNguoiDung={idNguoiDung}");

                    var sanPham = db.SanPhams.Find(idSanPham);
                    if (sanPham == null)
                    {
                        return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                    }

                    var gioHangItem = db.GioHangs
                        .FirstOrDefault(gh => gh.idNguoiDung == idNguoiDung
                                            && gh.idSanPham == idSanPham
                                            && gh.Size == size);

                    if (gioHangItem != null)
                    {
                        gioHangItem.SoLuong += soLuong;
                    }
                    else
                    {
                        db.GioHangs.Add(new GioHang
                        {
                            idNguoiDung = idNguoiDung,
                            idSanPham = idSanPham,
                            SoLuong = soLuong,
                            Size = size
                        });
                    }
                    var changes = db.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Số lượng thay đổi: {changes.Count}");

                    db.SaveChanges();

                    var cartCount = db.GioHangs
                        .Where(gh => gh.idNguoiDung == idNguoiDung)
                        .Sum(gh => gh.SoLuong);

                    return Json(new { success = true, cartCount = cartCount });
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                string fullErrorMessage = string.Join("; ", errorMessages);
                System.Diagnostics.Debug.WriteLine(fullErrorMessage);

                return Json(new { success = false, message = fullErrorMessage });
            }
            catch (Exception ex)
            {
                // Log lỗi
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return Json(new
                {
                    success = false,
                    message = "Lỗi server: " + ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}