using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using WebAzure.Models;
using WebAzure.Models.HomeModel;

namespace WebAzure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var client = new RestClient();

            CreateHomeViewModel createHomeViewModel = new CreateHomeViewModel();

            var requestFriend = new RestRequest("http://localhost:5003/api/friends", DataFormat.Json);
            var responseFriend = client.Get<List<Friend>>(requestFriend);
            createHomeViewModel.Friends = responseFriend.Data.Count;
            var requestCountry = new RestRequest("http://localhost:5000/api/countries", DataFormat.Json);
            var responseCountry = client.Get<List<Country>>(requestCountry);
            createHomeViewModel.Countries = responseCountry.Data.Count;
            var requestState = new RestRequest("http://localhost:5000/api/states", DataFormat.Json);
            var responseState = client.Get<List<State>>(requestState);
            createHomeViewModel.States = responseState.Data.Count;

            return View(createHomeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
