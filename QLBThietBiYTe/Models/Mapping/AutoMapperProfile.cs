using AutoMapper;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using System.Globalization;

namespace QLBThietBiYTe.Models.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /*CreateMap<NhaCungCapMap, Nhacungcap>().ReverseMap();
            CreateMap<LoaiThietBiMap, Loaithietbi>().ReverseMap();
            CreateMap<ChiTietHoaDonMap, Chitiethoadon>().ReverseMap();

            CreateMap<HoaDonMap, Hoadon>()
            .ForMember(dest => dest.Ngaylap, opt => opt.MapFrom(src => src.Ngaylap != "" ? DateTime.ParseExact(src.Ngaylap, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
            .ForMember(dest => dest.Tongtien, opt => opt.MapFrom(src => src.Tongtien != null ? decimal.Parse(src.Tongtien) : (decimal?)null))
            .ReverseMap();

            CreateMap<KhoMap, Kho>().ReverseMap();
            CreateMap<TaiKhoanMap, Taikhoan>().ReverseMap();

            CreateMap<ThietBiMap, Thietbi>()
            .ForMember(dest => dest.Namsanxuat, opt => opt.MapFrom(src => src.Namsanxuat != "" ? DateTime.ParseExact(src.Namsanxuat, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
            .ForMember(dest => dest.Giaban, opt => opt.MapFrom(src => src.Giaban != null ? int.Parse(src.Giaban) : (int?)null))
            .ForMember(dest => dest.Giamua, opt => opt.MapFrom(src => src.Giamua != null ? int.Parse(src.Giamua) : (int?)null))
            .ReverseMap();*/
        }
    }
}
