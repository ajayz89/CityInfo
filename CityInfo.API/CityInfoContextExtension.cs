using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextExtension
    {
        public static void EnsureSeedDataForContext(this CityInfoContext ctx)
        {
            if (ctx.Cities.Any())
            {

                return;
            }

            List<City> cities = new List<City>()
            {
                new City() {
                Name = "Rome",
                Description = "History",
                PointOfInterests = new List<PointOfInterest>() {
                  new PointOfInterest() {
                  Name = "Colosseo",
                  Description = "Battles"
                  },
                  new PointOfInterest() {
                  Name = "San pietro",
                  Description = "Church"
                  }
                }
                },
                new City() {
                Name = "Milan",
                Description = "Fashion",
                PointOfInterests = new List<PointOfInterest>() {
                  new PointOfInterest() {
                  Name = "Duomo",
                  Description = "History"
                  },
                  new PointOfInterest() {
                  Name = "Gallery",
                  Description = "Fashion"
                  }
                }
                }
            };

            ctx.Cities.AddRange(cities);
            ctx.SaveChanges();

        }
    }
}
