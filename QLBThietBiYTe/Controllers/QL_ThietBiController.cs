using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    /*[SessionAuthorize]*/
    [Route("QuanLy/[controller]")]
    public class QL_ThietBiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlThietBiServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_ThietBiController(IQlThietBiServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
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
        [HttpPost("getDSThietBi")]
        public async Task<IActionResult> DanhSachThietBi()
        {
            var rs = await _services.DanhSachThietBi();
            return Ok(rs);
        }
        [HttpPost("GetMaThietBi")]
        public async Task<IActionResult> getMaThietBi(string matb)
        {
            var rs = await _services.GetMaThietBi(matb);
            return Ok(rs);
        }
        [HttpPost("CreateThietBi")]
        public async Task<IActionResult> CreateThietBi(ThietBiMap modelMap)
        {
            var rs = await _services.CreateThietBi(modelMap);
            return Ok(rs);
        }
        [HttpPost("UpdateThietBi")]
        public async Task<IActionResult> UpdateThietBi(ThietBiMap modelMap)
        {
            var rs = await _services.UpdateThietBi(modelMap);
            return Ok(rs);
        }
        [HttpPost("DeleteThietBi")]
        public async Task<IActionResult> DeleteThietBi(string matb)
        {
            var rs = await _services.DeleteThietBi(matb);
            return Ok(rs);
        }
        
    }
}
