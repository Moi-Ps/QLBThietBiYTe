using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;

namespace QLBThietBiYTe.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly ThietBiYTeContext _context;
        public DangNhapController(ThietBiYTeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string userName, string passWord)
        {
            // Kiểm tra thông tin tài khoản
            var user = _context.Taikhoans.FirstOrDefault(u => u.Username == userName && u.PassWord == passWord);

            if (user != null)
            {
                // Lưu trạng thái đăng nhập vào Session
                HttpContext.Session.SetString("UserName", user.Username);
                HttpContext.Session.SetString("Role", user.Role ?? "");

                // Chuyển hướng về trang chính
                return RedirectToAction("Index", "Home");
            }

            // Nếu đăng nhập thất bại
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return View();
        }

        public IActionResult Logout()
        {
            // Xóa thông tin đăng nhập khỏi Session
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
