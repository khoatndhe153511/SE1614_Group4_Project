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
                .ForMember(des => des.Id, otp => otp.MapFrom(src => Guid.NewGuid()))
                .ForMember(des => des.Role, otp => otp.MapFrom(src => Constants.Role.User));
        }
    }
}