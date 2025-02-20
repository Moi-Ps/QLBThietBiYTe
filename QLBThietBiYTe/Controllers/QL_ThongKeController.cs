using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Services;
using QLBThietBiYTe.Services.QuanLyServices;
using System.Globalization;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace QLBThietBiYTe.Controllers
{
    [SessionAuthorize]
    [Route("QuanLy/[controller]")]
    public class QL_ThongKeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQlThongKeServices _services;
        private readonly ThietBiYTeContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private static IWebHostEnvironment _hostingEnvironment;
        public QL_ThongKeController(IQlThongKeServices services, ILogger<HomeController> logger, ThietBiYTeContext context, IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment)
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
        [HttpPost("PDFtkDoanhThu")]
        public async Task<dynamic> PDFtkDoanhThu(string tuNgay, string denNgay)
        {
            var data = await _services.tkDoanhThu(tuNgay, denNgay);

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
            
            PartialViewResult partialViewResult = PartialView("PDFtkDoanhThu", data);
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
