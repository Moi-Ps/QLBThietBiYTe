using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_LoaiThietBiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlLoaiThietBiServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_LoaiThietBiController(IQlLoaiThietBiServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _services = services;
            _context = context;
            _converter = converter;
            _viewEngine = viewEngine;
            _hostingEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("getDSLTB")]
        public async Task<IActionResult> DanhSachLoaiThietBi()
        {
            var rs = await _services.DanhSachLoaiThietBi();
            return Ok(rs);
        }
        [HttpPost("getMaLoai")]
        public async Task<IActionResult> getMaLoai(string maLTB)
        {
            var rs = await _services.GetMaLoai(maLTB);
            return Ok(rs);
        }
        [HttpPost("CreateLoaiThietBi")]
        public async Task<IActionResult> CreateLoaiThietBi(LoaiThietBiMap modelMap)
        {
            var rs = await _services.CreateLoaiThietBi(modelMap);
            return Ok(rs);
        }
        [HttpPost("updateLoaiThietBi")]
        public async Task<IActionResult> UpdateLoaiThietBi(LoaiThietBiMap modelMap)
        {
            var rs = await _services.UpdateLoaiThietBi(modelMap);
            return Ok(rs);
        }
        [HttpPost("DeleteLTB")]
        public async Task<IActionResult> DeleteLoaiThietBi(string maLTB)
        {
            var rs = await _services.DeleteLoaiThietBi(maLTB);
            return Ok(rs);
        }
    }
}
