using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>() {
              new CityDto(){
              Id = 1,
              Name = "New york City",
              Description = "City That Never Sleeps",
              PointOfInterests = new List<PointOfInterestDto>() {
              new PointOfInterestDto(){
                  Id = 1,
                  Name = "Statue Of Liberty",
                  Description = "Rapresents Freedom"
              },
              new PointOfInterestDto(){
                  Id = 2,
                  Name = "Central Park",
                  Description = "Park.."
              }
              }
              },
              new CityDto(){
              Id = 2,
              Name = "Rome",
              Description = "Impressive culture",
               PointOfInterests = new List<PointOfInterestDto>() {
              new PointOfInterestDto(){
                  Id = 1,
                  Name = "Colosseum",
                  Description = "Battles.."
              },
              new PointOfInterestDto(){
                  Id = 2,
                  Name = "Pantheon",
                  Description = "Roman Church.."
              }
              }
              }
            };
        }
    }
}

