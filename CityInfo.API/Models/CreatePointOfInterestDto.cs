using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class CreatePointOfInterestDto
    {
        [Required (ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage ="Max length is 50" )] 
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage ="Max length is 100" )] 
        public string Description { get; set; }
    }
}
