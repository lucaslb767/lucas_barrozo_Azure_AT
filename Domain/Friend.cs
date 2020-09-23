using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Friend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime Bday { get; set; }

        //[JsonIgnore]
        public virtual Country Country { get; set; }
        //[JsonIgnore]
        public virtual State State { get; set; }
        //[JsonIgnore]
        public virtual IList<Brother> Friends { get; set; }
    }

    public class FriendResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime Bday { get; set; }
        public virtual Country Country { get; set; }
        
        public virtual State State { get; set; }
       
        public virtual IList<Brother> Friends { get; set; }
    }
}
