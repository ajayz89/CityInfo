using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestControllercs : Controller
    { 
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetpointOfInterests(int cityId) {
            var foundCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (foundCity == null) {
                return NotFound();
            }

            return Ok(foundCity.PointOfInterests);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}")]
        public IActionResult GetSinglepointOfInterests(int cityId, int id)
        {
            var foundCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (foundCity == null)
            {
                return NotFound();
            }

            var pOi = foundCity.PointOfInterests.FirstOrDefault(x => x.Id == id);
            if (pOi == null) {
                return NotFound();
            }

            return Ok(pOi);
        }
    }
}
