using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        bool CityExists(int cityId);
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool includePointsOfInterest);
        bool PointOfInterestExists(int cityId, int pointOfInterestId);

        IEnumerable<PointOfInterest> GetPointOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId);
        void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest);
        bool Save();
    }
}
