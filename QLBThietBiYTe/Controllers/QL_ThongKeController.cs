using Microsoft.AspNetCore.Mvc;
using QLBThietBiYTe.Services;

namespace QLBThietBiYTe.Controllers
{
    [SessionAuthorize]
    public class QL_ThongKeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
