using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationAboutCity.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {               // FROM               //  TO
            CreateMap<Models.City, ModelsDTO.CityWithoutPointsOfInterestDto>();
            CreateMap<Models.City, Models.CityDto>();
        }
    }
}
