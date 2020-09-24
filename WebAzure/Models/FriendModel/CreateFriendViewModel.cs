using Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAzure.Models.FriendModel
{
    public class CreateFriendViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime Bday { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
    }
}
