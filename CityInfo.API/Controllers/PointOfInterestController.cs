using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private IMailService _mailService;
        private IMapper _mapper;
        private ICityInfoRepository _cityInfoRep; 

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRep, IMapper mapper)
        {
            // Constructor Injection 
            _logger = logger; 
            _mailService = mailService;
            _cityInfoRep = cityInfoRep;
            _mapper = mapper;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetpointOfInterests(int cityId)
        {
            try
            {

                if (!_cityInfoRep.CityExists(cityId))
                {
                    _logger.LogInformation($" {cityId }  City not found");
                    return NotFound();
                }
                var foundRes = _cityInfoRep.GetPointOfInterestForCity(cityId);
                var results = _mapper.Map<IEnumerable<PointOfInterestDto>>(foundRes);
                return Ok(results);
            }
            catch (Exception ex) {

                _logger.LogCritical($"Error during getting Point of interests for city id  {cityId}. Error {ex}");
                return StatusCode(505, "Error providing info ...");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetSinglepointOfInterests(int cityId, int id)
        {
            if (!_cityInfoRep.CityExists(cityId))
                return NotFound();
            
            if (!_cityInfoRep.PointOfInterestExists(cityId,id))
            {
                return NotFound();
            }

            var foundres = _cityInfoRep.GetPointOfInterest(cityId, id);
            var results = _mapper.Map <PointOfInterestDto>(foundres);
            return Ok(results);
        }
        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterests(int cityId, [FromBody] CreatePointOfInterestDto pOi)
        {
            // if consumers values cant be serialized then object poi will be null and we have to tell that its a mistake that he makes hence : BadRequest

            if (pOi == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_cityInfoRep.CityExists(cityId))
            {
                return NotFound();
            }

            // getting Max POI id creating new POI
            var ToAdd = _mapper.Map<PointOfInterest>(pOi);
            _cityInfoRep.AddPointOfInterest(cityId, ToAdd);
            if (!_cityInfoRep.Save()) {
                return StatusCode(500, "Error during request");
            }

            var toReturn = _mapper.Map<PointOfInterestDto>(ToAdd);
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = toReturn.Id }, toReturn);
        }


        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterests(int cityId, int id, [FromBody] UpdatePointOfInterestDto pOi)
        {
            if (pOi == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var foundCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (foundCity == null)
            {
                return NotFound();
            }
            var foundPoI = foundCity.PointOfInterests.FirstOrDefault(x => x.Id == id);
            if (foundPoI == null)
                return NotFound();

            foundPoI.Name = pOi.Name;
            foundPoI.Description = pOi.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<UpdatePointOfInterestDto> patchDoc) {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var foundCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (foundCity == null)
            {
                return NotFound();
            }
            var foundPoI = foundCity.PointOfInterests.FirstOrDefault(x => x.Id == id);
            if (foundPoI == null)
                return NotFound();

            var pOiToPatch = new UpdatePointOfInterestDto()
            {
                Name = foundPoI.Name,
                Description = foundPoI.Description
            };

            //actually applies what comes form JsonInput
            patchDoc.ApplyTo(pOiToPatch,ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // we need to apply validation on patchedPoi too
            // name != desC

            if (pOiToPatch.Name == pOiToPatch.Description)
                ModelState.AddModelError("Description", "Name and Description must be different");

            // validate patched one
            TryValidateModel(pOiToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foundPoI.Name = pOiToPatch.Name;
            foundPoI.Description = pOiToPatch.Description;

            return NoContent();
        }


        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeleteSinglepointOfInterests(int cityId, int id)
        {
            var foundCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (foundCity == null)
            {
                return NotFound();
            }

            var pOi = foundCity.PointOfInterests.FirstOrDefault(x => x.Id == id);
            if (pOi == null)
            {
                return NotFound();
            }
            foundCity.PointOfInterests.Remove(pOi);
            _mailService.Send("POI DELETED", $"POI {pOi.Name} has been deleted");
            return NoContent();
        }
    }
}
