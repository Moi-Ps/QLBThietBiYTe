using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services.QuanLyServices;
using System.Collections.Generic;
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
        [HttpPost("read")]
        public async Task<IActionResult> Read()
        {
            var rs = await _services.read();
            return Ok(rs);
        }
        [HttpPost("CreateHoaDon")]
        public async Task<IActionResult> CreateHoaDon([FromBody] HoaDonRequest request)
        {
            var rs = await _services.createHoaDon(request.hoDon, request.chiTietHoaDon);
            return Ok(rs);
        }
        [HttpPost("getHoaDon")]
        public async Task<IActionResult> GetHoaDon(string maHoaDon)
        {
            var rs = await _services.getHoaDon(maHoaDon);
            return Ok(rs);
        }
        [HttpPost("deleteHoaDon")]
        public async Task<IActionResult> DeleteHoaDon(string maHoaDon)
        {
            var rs = await _services.DeleteHoaDon(maHoaDon);
            return Ok(rs);
        }
        [HttpPost("deleteChiTietHoaDon")]
        public async Task<IActionResult> DeleteChiTietHoaDon(string maChiTiet)
        {
            var rs = await _services.DeleteChiTietHoaDon(maChiTiet);
            return Ok(rs);
        }
    }
    public class HoaDonRequest
    {
        public HoaDonMap hoDon { get; set; }
        public List<ChiTietHoaDonMap> chiTietHoaDon { get; set; }
    }


}
