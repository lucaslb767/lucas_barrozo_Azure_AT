using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Country
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Flag { get; set; }
        [JsonIgnore]
        public virtual IList<State> States { get; set; }
    }
    public class CountryResponse
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Flag { get; set; }
        public virtual IList<State> States { get; set; }
    }
}
