using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlHoaDonServices
    {
        Task<dynamic> createHoaDon(HoaDonMap hoaDonMap);
        Task<dynamic> updateHoaDon(HoaDonMap hoaDonMap);
        Task<dynamic> createCTHoaDon(List<ChiTietHoaDonMap> chiTietHoaDonMap);
        Task<dynamic> updateCTHoaDon(List<ChiTietHoaDonMap> chiTietHoaDonMap);

        //Task<dynamic> read();
        //Task<dynamic> getHoaDon(string maHoaDon);
        //Task<dynamic> deleteHoaDon(string maHoaDon);
        //Task<dynamic> deleteCtHoaDon(string maCTHoaDon);
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

        public async Task<dynamic> createHoaDon(HoaDonMap hoaDonMap)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hoaDon = _mapper.Map<Hoadon>(hoaDonMap);
                await _context.Hoadons.AddAsync(hoaDon);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new
                {
                    statusCode = 200,
                    message = "Thêm hóa đơn thành công",
                    hoaDon
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new { statusCode = 500, message = "Lỗi khi thêm hóa đơn" };
            }
        }

        public async Task<dynamic> updateHoaDon(HoaDonMap hoaDonMap)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingHoaDon = await _context.Hoadons.FindAsync(hoaDonMap.Id);
                if (existingHoaDon == null)
                {
                    return new { statusCode = 404, message = "Hóa đơn không tồn tại" };
                }

                _mapper.Map(hoaDonMap, existingHoaDon);
                _context.Hoadons.Update(existingHoaDon);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new
                {
                    statusCode = 200,
                    message = "Cập nhật hóa đơn thành công",
                    existingHoaDon
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new { statusCode = 500, message = "Lỗi khi cập nhật hóa đơn" };
            }
        }
        public async Task<dynamic> createCTHoaDon(List<ChiTietHoaDonMap> chiTietHoaDonMap)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var chiTietHoaDons = _mapper.Map<List<Chitiethoadon>>(chiTietHoaDonMap);
                await _context.Chitiethoadons.AddRangeAsync(chiTietHoaDons);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new
                {
                    statusCode = 200,
                    message = "Thêm chi tiết hóa đơn thành công",
                    chiTietHoaDons
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new { statusCode = 500, message = "Lỗi khi thêm chi tiết hóa đơn" };
            }
        }

        public async Task<dynamic> updateCTHoaDon(List<ChiTietHoaDonMap> chiTietHoaDonMap)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var ctMap in chiTietHoaDonMap)
                {
                    var existingCT = await _context.Chitiethoadons.FindAsync(ctMap.Id);
                    if (existingCT == null)
                    {
                        return new { statusCode = 404, message = $"Chi tiết hóa đơn {ctMap.Id} không tồn tại" };
                    }

                    _mapper.Map(ctMap, existingCT);
                    _context.Chitiethoadons.Update(existingCT);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new { statusCode = 200, message = "Cập nhật chi tiết hóa đơn thành công" };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new { statusCode = 500, message = "Lỗi khi cập nhật chi tiết hóa đơn" };
            }
        }
    }
}
