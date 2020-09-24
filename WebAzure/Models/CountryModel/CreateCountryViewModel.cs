using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAzure.Models.CountryModel
{
    public class CreateCountryViewModel
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
