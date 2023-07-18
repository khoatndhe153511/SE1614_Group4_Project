using AutoMapper;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Utils;

namespace SE1614_Group4_Project_API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(des => des.Id,
                    otp => otp.MapFrom(src => Guid.NewGuid()))
                .ForMember(des => des.Role,
                    otp => otp.MapFrom(src => Constants.Role.User))
                .ForMember(des => des.Name,
                    otp => otp.MapFrom(src => src.UserName));

            CreateMap<BookmarkDTO, Bookmark>()
                .ForMember(des => des.CreatedAt,
                    opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(des => des.ModifiedAt,
                    opt => opt.MapFrom(x => DateTime.Now));
        }
    }
}