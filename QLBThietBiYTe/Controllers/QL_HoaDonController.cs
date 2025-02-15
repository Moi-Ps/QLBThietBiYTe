using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_HoaDonController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlHoaDonServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_HoaDonController(IQlHoaDonServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _services = services;
            _context = context;
            _converter = converter;
            _viewEngine = viewEngine;
            _hostingEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("updateCTHoaDon")]
        public async Task<IActionResult> Update(List<ChiTietHoaDonMap> chiTietHoaDonMap)
        {
            var rs = await _services.updateCTHoaDon(chiTietHoaDonMap);
            return Ok(rs);
        }
    }
   
}
