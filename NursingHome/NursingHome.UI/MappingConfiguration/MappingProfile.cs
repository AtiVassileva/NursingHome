using AutoMapper;
using NursingHome.DAL.Models;
using NursingHome.UI.Models.User;

namespace NursingHome.UI.MappingConfiguration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeInfo, EmployeeRegisterModel>()
                .ReverseMap();
            CreateMap<ResidentInfo, ResidentRegisterModel>()
                .ReverseMap();
        }
    }
}