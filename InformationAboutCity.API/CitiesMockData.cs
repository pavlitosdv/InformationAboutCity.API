using InformationAboutCity.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationAboutCity.API
{
    public class CitiesMockData
    {
        public static CitiesMockData Current { get; } = new CitiesMockData();

        public List<CityDto> Cities { get; set; }

        public CitiesMockData()
        {
            // init dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                     Id = 1,
                     Name = "New York City",
                     Description = "The one with that big park."
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished."
                },
                new CityDto()
                {
                    Id= 3,
                    Name = "Paris",
                    Description = "The one with that big tower."
                }
            };
        }
    }
}
