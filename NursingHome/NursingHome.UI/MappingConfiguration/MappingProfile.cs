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
            CreateMap<EmployeeInfo, EmployeeEditModel>()
                .ReverseMap();
            CreateMap<ApplicationUser, ResidentEditModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus))
                .ForMember(dest => dest.DateAdmitted, opt => opt.MapFrom(src => src.ResidentInfo!.DateAdmitted))
                .ForMember(dest => dest.DateDischarged, opt => opt.MapFrom(src => src.ResidentInfo!.DateDischarged))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.ResidentInfo!.DateOfBirth))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ResidentInfo!.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ResidentInfo!.Gender))
                .ForMember(dest => dest.DietNumber, opt => opt.MapFrom(src => src.ResidentInfo!.DietNumber))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.ResidentInfo!.RoomType))
                .ForMember(dest => dest.EmployeeManagerId,
                    opt => opt.MapFrom(src => src.ResidentInfo!.EmployeeManagerId))
                .ForMember(dest => dest.GpName, opt => opt.MapFrom(src => src.ResidentInfo!.GpName))
                .ForMember(dest => dest.GpLocation, opt => opt.MapFrom(src => src.ResidentInfo!.GpLocation))
                .ForMember(dest => dest.GpPhoneNumber, opt => opt.MapFrom(src => src.ResidentInfo!.GpPhoneNumber))
                .ForMember(dest => dest.FamilyMemberName, opt => opt.MapFrom(src => src.ResidentInfo!.FamilyMemberName))
                .ForMember(dest => dest.FamilyMemberPhoneNumber,
                    opt => opt.MapFrom(src => src.ResidentInfo!.FamilyMemberPhoneNumber))
                .ForMember(dest => dest.Pension, opt => opt.MapFrom(src => src.ResidentInfo!.Pension))
                .ForMember(dest => dest.Rent, opt => opt.MapFrom(src => src.ResidentInfo!.Rent))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.ResidentInfo!.Salary))
                .ForMember(dest => dest.OtherIncome, opt => opt.MapFrom(src => src.ResidentInfo!.OtherIncome))
                .ForMember(dest => dest.HasInheritance, opt => opt.MapFrom(src => src.ResidentInfo!.HasInheritance))
                .ForMember(dest => dest.HasSupportContract,
                    opt => opt.MapFrom(src => src.ResidentInfo!.HasSupportContract))
                .ForMember(dest => dest.HasRealEstateSale,
                    opt => opt.MapFrom(src => src.ResidentInfo!.HasRealEstateSale))
                .ForMember(dest => dest.HasDonation, opt => opt.MapFrom(src => src.ResidentInfo!.HasDonation));

            CreateMap<ResidentEditModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus))

                .AfterMap((src, dest) =>
                {
                    if (dest.ResidentInfo == null)
                        dest.ResidentInfo = new ResidentInfo();

                    dest.ResidentInfo.DateAdmitted = src.DateAdmitted ?? DateTime.MinValue;
                    dest.ResidentInfo.DateDischarged = src.DateDischarged;
                    dest.ResidentInfo.DateOfBirth = src.DateOfBirth ?? DateTime.MinValue;
                    dest.ResidentInfo.PhoneNumber = src.PhoneNumber;
                    dest.ResidentInfo.Gender = src.Gender;
                    dest.ResidentInfo.DietNumber = src.DietNumber;
                    dest.ResidentInfo.RoomType = src.RoomType;
                    dest.ResidentInfo.EmployeeManagerId = src.EmployeeManagerId!;
                    dest.ResidentInfo.GpName = src.GpName!;
                    dest.ResidentInfo.GpLocation = src.GpLocation!;
                    dest.ResidentInfo.GpPhoneNumber = src.GpPhoneNumber!;
                    dest.ResidentInfo.FamilyMemberName = src.FamilyMemberName!;
                    dest.ResidentInfo.FamilyMemberPhoneNumber = src.FamilyMemberPhoneNumber!;
                    dest.ResidentInfo.Pension = src.Pension;
                    dest.ResidentInfo.Rent = src.Rent;
                    dest.ResidentInfo.Salary = src.Salary;
                    dest.ResidentInfo.OtherIncome = src.OtherIncome;
                    dest.ResidentInfo.HasInheritance = src.HasInheritance;
                    dest.ResidentInfo.HasSupportContract = src.HasSupportContract;
                    dest.ResidentInfo.HasRealEstateSale = src.HasRealEstateSale;
                    dest.ResidentInfo.HasDonation = src.HasDonation;
                });

            CreateMap<ApplicationUser, EmployeeEditModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus))
                .ForMember(dest => dest.EmployeePosition,
                    opt => opt.MapFrom(src => src.EmployeeInfo!.EmployeePosition));

            CreateMap<EmployeeEditModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus))

                .AfterMap((src, dest) =>
                {
                    if (dest.EmployeeInfo == null)
                        dest.EmployeeInfo = new EmployeeInfo();

                    dest.EmployeeInfo.EmployeePosition = src.EmployeePosition;
                });
        }
    }
}