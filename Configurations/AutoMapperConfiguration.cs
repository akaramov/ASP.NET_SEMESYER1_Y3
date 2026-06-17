using APAERMENT_LAST_API.DTOs.Requests;
using APAERMENT_LAST_API.DTOs.Responses;
using APAERMENT_LAST_API.Models;
using AutoMapper;

namespace APAERMENT_LAST_API.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<BuildingResDto, Building>().ReverseMap();
            CreateMap<BuildingReqDto, Building>().ReverseMap();
        }
    }
}
