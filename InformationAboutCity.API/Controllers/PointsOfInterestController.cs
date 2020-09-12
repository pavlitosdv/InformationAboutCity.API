using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InformationAboutCity.API.Models;
using InformationAboutCity.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InformationAboutCity.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService, ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
           
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                // throw new Exception("Exception example.");
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when " +
                        $"accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

       
        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

            #region old code obsolete
            //var city = CitiesMockData.Current.Cities
            //    .FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //// find point of interest
            //var pointOfInterest = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == id);

            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);
            #endregion
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId,[FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description","The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            #region in memory data (commented out)
            //var city = CitiesMockData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var maxPointOfInterestId = CitiesMockData.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            #endregion

            #region manually mapping

            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //city.PointsOfInterest.Add(finalPointOfInterest);
            //return CreatedAtRoute("GetPointOfInterest", new { cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);
            //};

            #endregion

            var finalPointOfInterest = _mapper.Map<Models.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            // if something goes wrong here it will produces a server 500 error
            _cityInfoRepository.Save();

            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            // name of the IActionResult   // we pass into an anonymous object the parameters   // finalPointOfInterest is the newly created object passed into the body
            return CreatedAtRoute("GetPointOfInterest", new { cityId, id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description","The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #region old code and manually mapped
            //var city = CitiesMockData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(p => p.Id == id);
            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;

            //return NoContent(); // the means that the request completed successfully
            #endregion

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
           [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc) // we Use the JsonPatchDocument 
        {
            #region in-memory data obsolete
            //var city = CitiesMockData.Current.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == id);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}
            #endregion

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            #region manually mapped (commented out)
            //var pointOfInterestToPatch =
            //       new PointOfInterestForUpdateDto()
            //       {
            //           Name = pointOfInterestFromStore.Name,
            //           Description = pointOfInterestFromStore.Description
            //       };


            //patchDoc.ApplyTo(pointOfInterestToPatch, ModelState); // we apply the patch. We pass the Patch object and the ModelState for validation

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            //{
            //    ModelState.AddModelError("Description", "The provided description should be different from the name.");
            //}

            ////we add this in order to validate the  > pointOfInterestToPatch < because the 
            ////ModelState.IsValid above validates the PointOfInterestForUpdateDto
            //if (!TryValidateModel(pointOfInterestToPatch))
            //{
            //    return BadRequest(ModelState);
            //}


            //PointOfInterestDto             // PointOfInterestForUpdateDto
            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            #endregion

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState); // we apply the patch. We pass the Patch object and the ModelState for validation

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description","The provided description should be different from the name.");
            }

            //we add this in order to validate the  > pointOfInterestToPatch < because the 
            //ModelState.IsValid above validates the PointOfInterestForUpdateDto
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            #region old data using in memory
            //var city = CitiesMockData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            //city.PointsOfInterest.Remove(pointOfInterestFromStore);

            //return NoContent();
            #endregion

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            _cityInfoRepository.Save();

            _mailService.Send("Point of interest deleted.",
                    $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }
    }
}
