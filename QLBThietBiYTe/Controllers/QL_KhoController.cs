using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Services;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_KhoController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlKhoServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_KhoController(IQlKhoServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
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
        [HttpPost("getDSKho")]
        public async Task<IActionResult> DSKho()
        {
            var rs = await _services.DanhSachKho();
            return Ok(rs);
        }
        [HttpPost("pdfKho")]
        public async Task<dynamic> PDFKho()
        {
            var data = await _services.PDFKho();

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = WkHtmlToPdfDotNet.PaperKind.A4,
                        Margins =
                        {
                            Top = 0.5,
                            Left = 0.5,
                            Right = 0.5,
                            Bottom = 0.5,
                            Unit = Unit.Centimeters
                        },
                    },
            };

            PartialViewResult partialViewResult = PartialView("PDFKho", data);
            string viewContent = CommonServices.ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            doc.Objects.Add(new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = viewContent,
                WebSettings = { DefaultEncoding = "utf-8",
                LoadImages = true
                },
                FooterSettings = {
                               Right = "[page]",
                           }
            });

            var pdfBytes = _converter.Convert(doc);
            return File(pdfBytes, "application/pdf", "output.pdf");
        }
    }
}
