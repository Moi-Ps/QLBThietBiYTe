using Microsoft.AspNetCore.Mvc;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services;

/*[SessionAuthorize]*/
[Route("QuanLy/[controller]")]
public class QL_HoaDonController : Controller
{
    private readonly IQlHoaDonServices _services;

    public QL_HoaDonController(IQlHoaDonServices services)
    {
        _services = services;
    }

    [HttpGet]
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
    public async Task<IActionResult> GetHoaDon([FromBody] GetHoaDonRequest req)
    {
        var rs = await _services.getHoaDon(req.maHoaDon);
        return Ok(rs);
    }

    [HttpPost("deleteHoaDon")]
    public async Task<IActionResult> DeleteHoaDon([FromBody] DeleteHoaDonRequest req)
    {
        var rs = await _services.DeleteHoaDon(req.maHoaDon);
        return Ok(rs);
    }

    [HttpPost("deleteChiTietHoaDon")]
    public async Task<IActionResult> DeleteChiTietHoaDon([FromBody] DeleteChiTietRequest req)
    {
        var rs = await _services.DeleteChiTietHoaDon(req.maChiTiet);
        return Ok(rs);
    }
    public class HoaDonRequest
    {
        public HoaDonMap hoDon { get; set; }
        public List<ChiTietHoaDonMap> chiTietHoaDon { get; set; }
    }

    public class GetHoaDonRequest
    {
        public string maHoaDon { get; set; }
    }

    public class DeleteHoaDonRequest
    {
        public string maHoaDon { get; set; }
    }

    public class DeleteChiTietRequest
    {
        public string maChiTiet { get; set; }
    }
}


