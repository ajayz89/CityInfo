using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }
        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(x => x.Name);
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
                return _context.Cities.Include(c => c.PointOfInterests).Where(x => x.Id == cityId).FirstOrDefault();

            return _context.Cities.Where(x => x.Id == cityId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest.Where(x => x.CityId == cityId);
        }

        public PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest.Where(x => x.CityId == cityId && x.Id == pointOfInterestId).FirstOrDefault();
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Where(x=> x.Id == cityId).FirstOrDefault() == null ? false : true;
        }

        public bool PointOfInterestExists(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest.Where(x => x.CityId == cityId && x.Id == pointOfInterestId).FirstOrDefault() == null ? false : true;
        }

        public void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointOfInterests.Add(pointOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
