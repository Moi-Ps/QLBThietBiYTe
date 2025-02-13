using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_NhaCungCapController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlNhaCungCapServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_NhaCungCapController(IQlNhaCungCapServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
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
        [HttpPost("getDSNCC")]
        public async Task<IActionResult> DanhSachNhaCungCap()
        {
            var rs = await _services.DanhSachNhaCungCap();
            return Ok(rs);
        }
        [HttpPost("getMaNCC")]
        public async Task<IActionResult> getMaNCC(string mancc)
        {
            var rs = await _services.GetMaNCC(mancc);
            return Ok(rs);
        }
        [HttpPost("CreateNhaCungCap")]
        public async Task<IActionResult> CreateNhaCungCap(NhaCungCapMap modelMap)
        {
            var rs = await _services.CreateNhaCungCap(modelMap);
            return Ok(rs);
        }
        [HttpPost("UpdateNhaCungCap")]
        public async Task<IActionResult> UpdateNhaCungCap(NhaCungCapMap modelMap)
        {
            var rs = await _services.UpdateNhaCungCap(modelMap);
            return Ok(rs);
        }
        [HttpPost("DeleteNCC")]
        public async Task<IActionResult> DeleteNhaCungCap(string mancc)
        {
            var rs = await _services.deleteNhaCungCap(mancc);
            return Ok(rs);
        }
    }
}
