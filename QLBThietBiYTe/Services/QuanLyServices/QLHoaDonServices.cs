using Microsoft.EntityFrameworkCore;
using AutoMapper;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;

public interface IQlHoaDonServices
{
    Task<dynamic> createHoaDon(HoaDonMap hoaDonMap, List<ChiTietHoaDonMap> chiTietHoaDonMaps);
    Task<dynamic> read();
    Task<dynamic> getHoaDon(string maHoaDon);
    Task<dynamic> DeleteChiTietHoaDon(string maChiTiet);
    Task<dynamic> DeleteHoaDon(string maHoaDon);
}

public class QLHoaDonServices : IQlHoaDonServices
{
    private readonly ThietBiYTeContext _context;
    private readonly IMapper _mapper;

    public QLHoaDonServices(ThietBiYTeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<dynamic> read()
    {
        var data = await _context.Hoadons
            .Select(x => new
            {
                MaHoaDon = x.Mahoadon,
                TenKhachHang = x.Tenkhachhang,
                NgayLap = x.Ngaylap,
                TongTien = x.Tongtien
            }).ToListAsync();

        return data;
    }
    public async Task<dynamic> getHoaDon(string maHoaDon)
    {
        var data = await _context.Hoadons
            .Where(x => x.Mahoadon == maHoaDon)
            .Select(x => new
            {
                MaHoaDon = x.Mahoadon,
                TenKhachHang = x.Tenkhachhang,
                NgayLap = x.Ngaylap,
                TongTien = x.Tongtien,
                ChiTietHoaDon = x.Chitiethoadons.Select(ct => new
                {
                    MaChiTiet = ct.Machitiet,
                    MaThietBi = ct.Mathietbi,
                    TenThietBi = ct.MathietbiNavigation.Tenthietbi,
                    SoLuong = ct.Soluong,
                    GiaTien = ct.Giatien,
                    ThanhTien = ct.Thanhtien
                }).ToList()
            })
            .FirstOrDefaultAsync();

        return data;
    }

    public async Task<dynamic> createHoaDon(HoaDonMap hoaDonMap, List<ChiTietHoaDonMap> chiTietHoaDonMaps)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Lấy hóa đơn master (nếu có) hoặc tạo mới
            var master = await _context.Hoadons
                .Include(h => h.Chitiethoadons)
                .FirstOrDefaultAsync(h => h.Mahoadon == hoaDonMap.Mahoadon);

            if (master == null)
            {
                master = _mapper.Map<Hoadon>(hoaDonMap);
                master.Chitiethoadons = new List<Chitiethoadon>();
                await _context.Hoadons.AddAsync(master);
            }
            else
            {
                master.Tenkhachhang = hoaDonMap.Tenkhachhang;
                master.Ngaylap = hoaDonMap.Ngaylap;
                master.Tongtien = hoaDonMap.Tongtien;
                _context.Entry(master).State = EntityState.Modified;
                _context.Entry(master).Property(x => x.Id).IsModified = false;

                if (master.Chitiethoadons == null)
                {
                    master.Chitiethoadons = new List<Chitiethoadon>();
                }
            }

            // Dùng dictionary để lưu tổng delta của mỗi thiết bị (key = Mathietbi)
            Dictionary<string, int> deviceDelta = new Dictionary<string, int>();

            var ctList = _mapper.Map<List<Chitiethoadon>>(chiTietHoaDonMaps);

            foreach (var ct in ctList)
            {
                ct.Mahoadon = master.Mahoadon;
                ct.Thanhtien = (ct.Soluong ?? 0) * (ct.Giatien ?? 0);

                // Nếu có mã chi tiết (để kiểm tra update) thì thử lấy chi tiết cũ
                if (!string.IsNullOrEmpty(ct.Machitiet))
                {
                    var existingDetail = master.Chitiethoadons.FirstOrDefault(x => x.Machitiet == ct.Machitiet);
                    if (existingDetail != null)
                    {
                        // Lấy số lượng cũ trước khi cập nhật
                        int oldQuantity = existingDetail.Soluong ?? 0;
                        int newQuantity = ct.Soluong ?? 0;
                        int delta = newQuantity - oldQuantity;

                        // Cập nhật delta cho thiết bị
                        if (deviceDelta.ContainsKey(ct.Mathietbi))
                            deviceDelta[ct.Mathietbi] += delta;
                        else
                            deviceDelta[ct.Mathietbi] = delta;

                        // Cập nhật chi tiết
                        existingDetail.Soluong = newQuantity;
                        existingDetail.Giatien = ct.Giatien;
                        existingDetail.Thanhtien = ct.Thanhtien;
                        _context.Entry(existingDetail).State = EntityState.Modified;
                        _context.Entry(existingDetail).Property(x => x.Id).IsModified = false;
                    }
                    else
                    {
                        // Chi tiết mới: delta = new quantity (vì oldQuantity = 0)
                        int newQuantity = ct.Soluong ?? 0;
                        if (deviceDelta.ContainsKey(ct.Mathietbi))
                            deviceDelta[ct.Mathietbi] += newQuantity;
                        else
                            deviceDelta[ct.Mathietbi] = newQuantity;

                        master.Chitiethoadons.Add(ct);
                    }
                }
                else
                {
                    // Nếu không có mã chi tiết, coi như chi tiết mới
                    int newQuantity = ct.Soluong ?? 0;
                    if (deviceDelta.ContainsKey(ct.Mathietbi))
                        deviceDelta[ct.Mathietbi] += newQuantity;
                    else
                        deviceDelta[ct.Mathietbi] = newQuantity;
                    master.Chitiethoadons.Add(ct);
                }
            }

            // Tính lại tổng tiền từ chi tiết
            master.Tongtien = master.Chitiethoadons.Sum(ct => ct.Thanhtien ?? 0);

            // Lưu hóa đơn và chi tiết trước khi cập nhật kho
            await _context.SaveChangesAsync();

            // 3. Cập nhật lại kho: với mỗi thiết bị, trừ số lượng theo tổng delta
            foreach (var kvp in deviceDelta)
            {
                string deviceId = kvp.Key;
                int delta = kvp.Value; // số lượng cần trừ (nếu delta > 0) hoặc cần cộng lại (nếu delta < 0)
                var khoItem = await _context.Khos.FirstOrDefaultAsync(k => k.Mathietbi == deviceId);
                if (khoItem != null)
                {
                    int soLuongKho = khoItem.Soluong ?? 0;
                    // Nếu delta > 0: cần trừ kho, nếu delta < 0: cộng lại (trường hợp sửa giảm số lượng)
                    if (delta > 0)
                    {
                        if (soLuongKho < delta)
                        {
                            // Nếu không đủ số lượng, trả về thông báo lỗi
                            return new { message = $"Không đủ hàng tồn kho cho thiết bị {deviceId}. Tồn kho: {soLuongKho}, cần: {delta}." };
                        }
                        khoItem.Soluong = soLuongKho - delta;
                    }
                    else if (delta < 0)
                    {
                        // Nếu delta âm, nghĩa là giảm số lượng hóa đơn, cộng lại kho
                        khoItem.Soluong = soLuongKho - delta; // vì delta âm => cộng dần
                    }
                    // Nếu delta == 0 thì không thay đổi
                }
                else
                {
                    return new { message = $"Không tìm thấy kho cho thiết bị {deviceId}" };
                }
            }

            // Lưu thay đổi cho bảng kho
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new
            {
                message = "Thành công",
                master,
                ctHoaDon = master.Chitiethoadons
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new
            {
                error = "Thất bại",
                details = ex.InnerException?.Message ?? ex.Message
            };
        }
    }

    public async Task<dynamic> DeleteChiTietHoaDon(string maChiTiet)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var chiTiet = await _context.Chitiethoadons.FirstOrDefaultAsync(ct => ct.Machitiet == maChiTiet);
            if (chiTiet == null)
            {
                return new 
                {
                    message = "Chi tiết hóa đơn không tồn tại" 
                };
            }

            int soLuongDetail = chiTiet.Soluong ?? 0;
            string mathietbi = chiTiet.Mathietbi;
            string maHoaDon = chiTiet.Mahoadon;

            _context.Chitiethoadons.Remove(chiTiet);
            await _context.SaveChangesAsync();

            var khoItem = await _context.Khos.FirstOrDefaultAsync(k => k.Mathietbi == mathietbi);
            if (khoItem != null)
            {
                khoItem.Soluong = (khoItem.Soluong ?? 0) + soLuongDetail;
                await _context.SaveChangesAsync();
            }
            else
            {
                return new 
                { 
                    message = "Không tìm thấy kho cho thiết bị" 
                };
            }

            var hoaDon = await _context.Hoadons
                .Include(x => x.Chitiethoadons)
                .FirstOrDefaultAsync(h => h.Mahoadon == maHoaDon);
            if (hoaDon != null)
            {
                hoaDon.Tongtien = hoaDon.Chitiethoadons.Sum(x => x.Thanhtien ?? 0);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return new 
            { 
                message = "Thành công" 
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new
            {
                error = "Thất bại",
                details = ex.InnerException?.Message ?? ex.Message
            };
        }
    }


    public async Task<dynamic> DeleteHoaDon(string maHoaDon)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            
            var hoaDon = await _context.Hoadons
                .Include(h => h.Chitiethoadons)
                .FirstOrDefaultAsync(h => h.Mahoadon == maHoaDon);

            if (hoaDon == null)
            {
                return new { error = "Hóa đơn không tồn tại" };
            }

            
            if (hoaDon.Chitiethoadons != null && hoaDon.Chitiethoadons.Any())
            {
                _context.Chitiethoadons.RemoveRange(hoaDon.Chitiethoadons);
            }

            
            _context.Hoadons.Remove(hoaDon);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new { message = "Thành công" };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new
            {
                error = "Thất bại",
                details = ex.InnerException?.Message ?? ex.Message
            };
        }
    }

}
