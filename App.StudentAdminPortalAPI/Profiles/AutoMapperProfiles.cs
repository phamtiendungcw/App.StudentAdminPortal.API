using App.StudentAdminPortalAPI.DomainModels;
using App.StudentAdminPortalAPI.Profiles.AfterMaps;
using AutoMapper;

namespace App.StudentAdminPortalAPI.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DataModels.Student, DomainModels.Student>().ReverseMap();
            CreateMap<DataModels.Gender, DomainModels.Gender>().ReverseMap();
            CreateMap<DataModels.Address, DomainModels.Address>().ReverseMap();
            CreateMap<UpdateStudentRequest, DataModels.Student>().AfterMap<UpdateStudentRequestAfterMap>();
        }
    }
}