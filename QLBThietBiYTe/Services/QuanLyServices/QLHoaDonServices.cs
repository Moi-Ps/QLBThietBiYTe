using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Controllers;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlHoaDonServices
    {
        /*Task<dynamic> createHoaDon(HoaDonMap request);*/
        Task<dynamic> createHoaDon(HoaDonMap hoaDonMap, List<ChiTietHoaDonMap> chiTietHoaDonMaps);
        Task<dynamic> read();
        Task<dynamic> getHoaDon(string maHD);
        Task<dynamic> DeleteChiTietHoaDon(string maChiTiet);
        Task<dynamic> DeleteHoaDon(string maHoaDon);
    }
    public class QLHoaDonServices : IQlHoaDonServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;
        public QLHoaDonServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> read()
        {
            var data = await _context.Hoadons.
            Select(x => new
            {
                Id = x.Id,
                MaHoaDon = x.Mahoadon,
                TenKhachHang = x.Tenkhachhang,
                NgayLap = x.Ngaylap,
                TongTien = x.Tongtien
            }).ToListAsync();
            return data;
        }
        public async Task<dynamic> getHoaDon(string maHD)
        {
            var data = await _context.Hoadons
                .Where(x => x.Mahoadon == maHD)
                .Select(x => new
                {
                    MaHoaDon = x.Mahoadon,
                    TenKhachHang = x.Tenkhachhang,
                    NgayLap = x.Ngaylap,
                    TongTien = x.Tongtien,
                    ChiTietHoaDon = x.Chitiethoadons.Select(ct => new
                    {
                        MaChiTiet = ct.Machitiet,
                        TenThietBi = ct.MathietbiNavigation.Tenthietbi,
                        SoLuong = ct.Soluong,
                        GiaTien = ct.Giatien,
                        ThanhTien = ct.Thanhtien
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return data;
        }
        /*public async Task<dynamic> createHoaDon(HoaDonMap request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var hoaDon = await _context.Hoadons
                    .Include(h => h.Chitiethoadons)
                    .FirstOrDefaultAsync(h => h.Mahoadon == request.Mahoadon);

                if (hoaDon == null)
                {
                    hoaDon = _mapper.Map<Hoadon>(request);
                    hoaDon.Chitiethoadons = new List<Chitiethoadon>();
                    _context.Hoadons.Add(hoaDon);
                }
                else
                {
                    hoaDon.Tenkhachhang = request.Tenkhachhang;
                    hoaDon.Ngaylap = request.Ngaylap;
                    hoaDon.Tongtien = request.Tongtien;
                    _context.Entry(hoaDon).CurrentValues.SetValues(request);
                }

                // Danh sách chi tiết hóa đơn mới gửi lên
                var chiTietMoi = _mapper.Map<List<Chitiethoadon>>(request.Chitiethoadons);

                foreach (var ct in chiTietMoi)
                {
                    ct.Mahoadon = hoaDon.Mahoadon;
                    ct.Thanhtien = (ct.Soluong ?? 0) * (ct.Giatien ?? 0);

                    // Tìm chi tiết hóa đơn cũ theo `Machitiethoadon`
                    var chiTietCu = hoaDon.Chitiethoadons.FirstOrDefault(x => x.Machitiet == ct.Machitiet);

                    if (chiTietCu != null)
                    {
                        // Nếu chi tiết hóa đơn đã tồn tại, cập nhật số lượng & thành tiền
                        chiTietCu.Soluong = ct.Soluong;
                        chiTietCu.Giatien = ct.Giatien;
                        chiTietCu.Thanhtien = ct.Thanhtien;
                    }
                    else
                    {
                        // Nếu chi tiết hóa đơn không tồn tại, thêm mới
                        hoaDon.Chitiethoadons.Add(ct);
                    }
                }

                // Tính tổng tiền hóa đơn
                hoaDon.Tongtien = hoaDon.Chitiethoadons.Sum(ct => ct.Thanhtien ?? 0);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new
                {
                    message = "Thành công",
                    hoaDon
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new
                {
                    error = "Lỗi khi xử lý hóa đơn",
                    details = ex.InnerException?.Message ?? ex.Message
                };
            }
        }*/
        
        public async Task<dynamic> createHoaDon(HoaDonMap hoaDonMap, List<ChiTietHoaDonMap> chiTietHoaDonMaps)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                
                Hoadon master = await _context.Hoadons
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
                    
                    if (master.Chitiethoadons == null)
                    {
                        master.Chitiethoadons = new List<Chitiethoadon>();
                    }
                }
                List<Chitiethoadon> ctHoaDon = _mapper.Map<List<Chitiethoadon>>(chiTietHoaDonMaps);

                foreach (var ct in ctHoaDon)
                {
                    ct.Mahoadon = master.Mahoadon;
                    ct.Thanhtien = (ct.Soluong ?? 0) * (ct.Giatien ?? 0);
                    if (ct.Machitiet != null)
                    {
                        var chiTietCu = master.Chitiethoadons.FirstOrDefault(x => x.Machitiet == ct.Machitiet);

                        if (chiTietCu != null)
                        {
                            chiTietCu.Soluong = ct.Soluong;
                            chiTietCu.Giatien = ct.Giatien;
                            chiTietCu.Thanhtien = ct.Thanhtien;
                        }
                        else
                        {
                            master.Chitiethoadons.Add(ct);
                        }
                    }
                    else
                    {
                        master.Chitiethoadons.Add(ct);
                    }
                }
                master.Tongtien = master.Chitiethoadons.Sum(ct => ct.Thanhtien ?? 0);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new
                {
                    message = "Thành công",
                    master = master,
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


        public async Task<dynamic> DeleteHoaDon(string maHoaDon)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var hoaDon = await _context.Hoadons.FirstOrDefaultAsync(h => h.Mahoadon == maHoaDon);

                if (hoaDon == null)
                {
                    return new { error = "Hóa đơn không tồn tại" };
                }

                _context.Hoadons.Remove(hoaDon);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new { message = "Xóa hóa đơn và các chi tiết hóa đơn thành công" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new { error = "Lỗi khi xóa hóa đơn", details = ex.InnerException?.Message ?? ex.Message };
            }
        }

        public async Task<dynamic> DeleteChiTietHoaDon(string maChiTiet)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var chiTiet = await _context.Chitiethoadons.FirstOrDefaultAsync(ct => ct.Machitiet == maChiTiet);

                if (chiTiet == null)
                {
                    return new { error = "Chi tiết hóa đơn không tồn tại" };
                }

                string maHoaDon = chiTiet.Mahoadon; // Lưu mã hóa đơn để kiểm tra sau khi xóa
                _context.Chitiethoadons.Remove(chiTiet);
                await _context.SaveChangesAsync();

                var hoaDon = await _context.Hoadons.FirstOrDefaultAsync(h => h.Mahoadon == maHoaDon);
                if (hoaDon != null)
                {
                    // Kiểm tra xem hóa đơn còn chi tiết nào không
                    bool conChiTiet = await _context.Chitiethoadons.AnyAsync(ct => ct.Mahoadon == maHoaDon);

                    // Nếu còn chi tiết, cập nhật tổng tiền, nếu không thì set tổng tiền về 0
                    hoaDon.Tongtien = conChiTiet
                        ? await _context.Chitiethoadons
                            .Where(ct => ct.Mahoadon == maHoaDon)
                            .SumAsync(ct => ct.Thanhtien ?? 0)
                        : 0;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new { message = "Xóa chi tiết hóa đơn thành công" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new { error = "Lỗi khi xóa chi tiết hóa đơn", details = ex.InnerException?.Message ?? ex.Message };
            }
        }


    }
}
