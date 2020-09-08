using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationAboutCity.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        // we initialize it as a best practise in order to avoid null reference excemptions
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
         = new List<PointOfInterestDto>();

    }
}
