using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationAboutCity.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Models.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Models.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Models.PointOfInterest>()
                .ReverseMap();
        }
    }
}
