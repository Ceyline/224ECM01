using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;
using BCrypt.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace ThuongMaiDienTu.Controllers
{
    public class Register_LoginController : Controller
    {
        // GET: Register_Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Register_Login/TestDb - Kiểm tra kết nối cơ sở dữ liệu
        [HttpGet]
        public ActionResult TestDb()
        {
            try
            {
                using (var db = new trangsucbacEntities())
                {
                    var count = db.NguoiDungs.Count();
                    return Json(new { success = true, message = $"Kết nối cơ sở dữ liệu thành công. Số người dùng: {count}" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi TestDb: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = $"Lỗi kết nối cơ sở dữ liệu: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Register_Login/Register - Xử lý đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                using (var db = new trangsucbacEntities())
                {
                    // Kiểm tra kết nối database
                    try
                    {
                        db.Database.Connection.Open();
                        db.Database.Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi kết nối DB: {ex.Message}\nStackTrace: {ex.StackTrace}");
                        return Json(new { success = false, message = "Không thể kết nối cơ sở dữ liệu: " + ex.Message });
                    }

                    // Kiểm tra dữ liệu đầu vào
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                        System.Diagnostics.Debug.WriteLine($"Lỗi ModelState: {string.Join("; ", errors)}");
                        return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join("; ", errors) });
                    }

                    // Kiểm tra mật khẩu xác nhận
                    if (model.Password != model.ConfirmPassword)
                    {
                        return Json(new { success = false, message = "Mật khẩu xác nhận không khớp." });
                    }

                    // Kiểm tra email đã tồn tại
                    if (db.NguoiDungs.Any(u => u.Email == model.Email))
                    {
                        return Json(new { success = false, message = "Email đã được đăng ký." });
                    }

                    // Tạo người dùng mới
                    var user = new NguoiDung
                    {
                        HoTen = model.FullName,
                        SoDienThoai = model.Phone,
                        DiaChi = model.Address,
                        Email = model.Email,
                        MatKhau = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        PhanQuyen = "User"
                    };

                    // Thêm vào cơ sở dữ liệu
                    db.NguoiDungs.Add(user);
                    db.SaveChanges();

                    // Thiết lập session
                    Session["idNguoiDung"] = user.idNguoiDung;
                    Session["HoTen"] = user.HoTen;
                    Session["Email"] = user.Email;
                    Session["SoDienThoai"] = user.SoDienThoai;
                    Session["DiaChi"] = user.DiaChi;

                    return Json(new { success = true, message = "Đăng ký thành công!" });
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}");
                string fullErrorMessage = "Lỗi xác thực: " + string.Join("; ", errorMessages);
                System.Diagnostics.Debug.WriteLine(fullErrorMessage);
                return Json(new { success = false, message = fullErrorMessage });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi đăng ký: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        // POST: Register_Login/Login - Xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                using (var db = new trangsucbacEntities())
                {
                    // Kiểm tra kết nối database
                    try
                    {
                        db.Database.Connection.Open();
                        db.Database.Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi kết nối DB: {ex.Message}\nStackTrace: {ex.StackTrace}");
                        return Json(new { success = false, message = "Không thể kết nối cơ sở dữ liệu: " + ex.Message });
                    }

                    // Kiểm tra dữ liệu đầu vào
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                        System.Diagnostics.Debug.WriteLine($"Lỗi ModelState: {string.Join("; ", errors)}");
                        return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join("; ", errors) });
                    }

                    // Tìm người dùng theo email
                    var user = db.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);

                    if (user == null)
                    {
                        return Json(new { success = false, message = "Email không tồn tại." });
                    }

                    // Kiểm tra mật khẩu
                    if (!BCrypt.Net.BCrypt.Verify(model.Password, user.MatKhau))
                    {
                        return Json(new { success = false, message = "Mật khẩu không đúng." });
                    }

                    // Thiết lập session
                    Session["idNguoiDung"] = user.idNguoiDung;
                    Session["HoTen"] = user.HoTen;
                    Session["Email"] = user.Email;
                    Session["SoDienThoai"] = user.SoDienThoai;
                    Session["DiaChi"] = user.DiaChi;

                    return Json(new { success = true, message = "Đăng nhập thành công!" });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi đăng nhập: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
    }
}