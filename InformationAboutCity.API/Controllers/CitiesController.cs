using AutoMapper;
using InformationAboutCity.API.Models;
using InformationAboutCity.API.ModelsDTO;
using InformationAboutCity.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationAboutCity.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            #region without auto mapper (manually) - commnented out
            //var cityEntities = _cityInfoRepository.GetCities();

            //var results = new List<CityWithoutPointsOfInterestDto>();

            //foreach (var cityEntity in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = cityEntity.Id,
            //        Description = cityEntity.Description,
            //        Name = cityEntity.Name
            //    });
            //}

            //return Ok(results);
            #endregion

            var cityEntities = _cityInfoRepository.GetCities();

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

            //return new JsonResult(CitiesMockData.Current.Cities);

            #region mock data (obsolete)
            //return new JsonResult(new List<object>()
            //{
            //    new { id=1, Name= "City of New York" },
            //    new { id=2, Name= "Belgium City" }
            //});
            #endregion

        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            //Here will need to also add a Profile becasuse a City mapped to CityDTO, the CityDTO it includes
            // a list of PointOfInterestDto which has to be mapped with a PointOfInterest
            // if we do not do it it will throw an excemption
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
