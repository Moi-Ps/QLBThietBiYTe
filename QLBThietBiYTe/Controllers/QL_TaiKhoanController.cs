using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    [SessionAuthorize]
    [Route("QuanLy/[controller]")]
    public class QL_TaiKhoanController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlTaiKhoanServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_TaiKhoanController(IQlTaiKhoanServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
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
        [HttpPost("read")]
        public async Task<IActionResult> Read()
        {
            var rs = await _services.read();
            return Ok(rs);
        }
        [HttpPost("getTaiKhoan")]
        public async Task<IActionResult> GetTaiKhoan(string maTK)
        {
            var rs = await _services.getTaiKhoan(maTK);
            return Ok(rs);
        }
        [HttpPost("CreateTaiKhoan")]
        public async Task<IActionResult> CreateTaiKhoan(TaiKhoanMap modelMap)
        {
            var rs = await _services.createTaiKhoan(modelMap);
            return Ok(rs);
        }
        [HttpPost("UpdateTaiKhoan")]
        public async Task<IActionResult> UpdateTaiKhoan(TaiKhoanMap modelMap)
        {
            var rs = await _services.updateTaiKhoan(modelMap);
            return Ok(rs);
        }
        [HttpPost("DeleteTaiKhoan")]
        public async Task<IActionResult> DeleteTaiKhoan(string maTK)
        {
            var rs = await _services.deleteTaiKhoan(maTK);
            return Ok(rs);
        }
    }
}
