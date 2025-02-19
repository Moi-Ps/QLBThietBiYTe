using Microsoft.AspNetCore.Mvc;
using QLBThietBiYTe.Models;
using QLBThietBiYTe.Models.Entities;
using System;
using System.Linq;
using System.Net;

namespace QLBThietBiYTe.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly ThietBiYTeContext _context;
        public DangNhapController(ThietBiYTeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var user = HttpContext.Session.GetString("User");
            var loginTimeStr = HttpContext.Session.GetString("LoginTime");
            if (!string.IsNullOrEmpty(user) &&
                DateTime.TryParse(loginTimeStr, out DateTime loginTime) &&
                (DateTime.Now - loginTime).TotalMinutes <= 10)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string Username, string Password, bool rememberMe)
        {
            var account = _context.Taikhoans
                .FirstOrDefault(a => a.Username == Username && a.PassWord == Password);

            if (account != null)
            {
                
                HttpContext.Session.SetString("User", account.Username);
                HttpContext.Session.SetString("Role", account.Role);
                HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString());

                if (rememberMe)
                {
                    
                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(10),
                        HttpOnly = true,
                        IsEssential = true
                    };
                    Response.Cookies.Append("User", account.Username, options);
                }
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.ErrorMessage = "Tên tài khoản hoặc mật khẩu không đúng!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("User");
            return RedirectToAction("Index");
        }
    }
}
