using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QLBThietBiYTe.Models;
using QLBThietBiYTe.Services;

namespace QLBThietBiYTe.Controllers
{
    /*[SessionAuthorize]*/
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Kiểm tra session
           /* var user = HttpContext.Session.GetString("User");
            var role = HttpContext.Session.GetString("Role");
            var loginTimeStr = HttpContext.Session.GetString("LoginTime");
            if (string.IsNullOrEmpty(user) ||
                !DateTime.TryParse(loginTimeStr, out DateTime loginTime) ||
                (DateTime.Now - loginTime).TotalMinutes > 10)
            {

                HttpContext.Session.Clear();
                return RedirectToAction("Index", "DangNhap");
            }*/
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
