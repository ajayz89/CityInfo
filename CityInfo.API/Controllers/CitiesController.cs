using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller

    {
        private ICityInfoRepository _cityInfoRep;
        private IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRep, IMapper mapper)
        {
            _cityInfoRep = cityInfoRep;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            // return Ok (CitiesDataStore.Current.Cities);
            var cityEntities = _cityInfoRep.GetCities();
            //var results = new List<CityWithoutPOI>();
            //cityEntities.ToList().ForEach(x =>
            // results.Add(new CityWithoutPOI() {
            //  Id = x.Id,
            //  Name = x.Name,
            //  Description = x.Description
            // }));
            var results = _mapper.Map<IEnumerable<CityWithoutPOI>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleCity(int id, bool includePOI = false ) {
            if(!_cityInfoRep.CityExists(id))
                return NotFound();

            var city = _cityInfoRep.GetCity(id, includePOI);

            if (includePOI)
            {
                var result = _mapper.Map<CityDto>(city);
                return Ok(result);
            }
            else {

        
               var result = _mapper.Map<CityWithoutPOI>(city);
                return Ok(result);
            }

            //var requestedCity = CitiesDataStore.Current.Cities.FirstOrDefault(x=> x.Id == id);
            //if (requestedCity == null)
            //    return NotFound();
            //else
            //    return Ok(requestedCity);

        }
    }
}
