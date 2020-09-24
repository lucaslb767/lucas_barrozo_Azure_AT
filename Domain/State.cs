using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain
{
    public class State
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Flag { get; set; }
        //[JsonIgnore]
        public virtual Country Country { get; set; }

    }

    public class StateResponse
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Flag { get; set; }
        public virtual Country Country { get; set; }
    }
}
