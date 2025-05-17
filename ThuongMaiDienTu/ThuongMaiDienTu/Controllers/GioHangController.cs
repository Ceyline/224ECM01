using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;
using System.Data.Entity;

namespace ThuongMaiDienTu.Controllers
{
    public class GioHangController : Controller
    {
        private trangsucbacEntities db = new trangsucbacEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            int idNguoiDung = 2; // Tạm thời set cứng
            using (var db = new trangsucbacEntities())
            {
                var gioHang = db.GioHangs
                    .Where(gh => gh.idNguoiDung == idNguoiDung)
                    .Include(gh => gh.SanPham) // Đảm bảo SanPham được tải kèm theo GioHang
                    .ToList();

                return View(gioHang); // Trả về danh sách GioHang cho view
            }
        }
        [HttpPost]
        public ActionResult UpdateCart(int? productId, int quantity, string action)
        {
            try
            {
                if (productId == null || productId == 0)
                {
                    return Json(new { success = false, message = "Product ID không hợp lệ." });
                }

                int idNguoiDung = 2;

                using (var db = new trangsucbacEntities())
                {
                    var cartItem = db.GioHangs.FirstOrDefault(gh => gh.idNguoiDung == idNguoiDung && gh.idSanPham == productId);

                    if (cartItem == null)
                    {
                        return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
                    }

                    if (action == "update")
                    {
                        cartItem.SoLuong = quantity;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Cập nhật số lượng thành công." });
                    }
                    else if (action == "delete")
                    {
                        db.GioHangs.Remove(cartItem);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Xóa sản phẩm khỏi giỏ hàng thành công." });
                    }

                    return Json(new { success = false, message = "Hành động không hợp lệ." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }



        [HttpPost]
        public ActionResult ThemGioHang(int idSanPham, int soLuong, string size)
        {
            try
            {
                using (var db = new trangsucbacEntities())
                {
<<<<<<< HEAD
                    if (Session["idNguoiDung"] == null)
                    {
                        return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng." });
                    }

                    int idNguoiDung = (int)Session["idNguoiDung"];

                    //int idNguoiDung = 2; // Tạm thời set cứng

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
                    //var changes = db.ChangeTracker.Entries()
                    //    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                    //    .ToList();
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