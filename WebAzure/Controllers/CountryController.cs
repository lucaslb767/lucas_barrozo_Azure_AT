using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using RestSharp;
using WebAzure.Models.CountryModel;

namespace WebAzure.Controllers
{
    public class CountryController : Controller
    {
        // GET: CountryController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5000/api/countries", DataFormat.Json);
            var response = client.Get<List<Country>>(request);

            return View(response.Data);
        }

        // GET: CountryController/Details/5
        public ActionResult Details(int id)
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5000/api/countries/" +id, DataFormat.Json);
            var response = client.Get<Country>(request);
            return View(response.Data);
        }

        // GET: CountryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CountryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCountryViewModel createCountryViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5000/api/countries", DataFormat.Json);
                var urlPhoto = UploadPhotoFriend(createCountryViewModel.PhotoFile);
                createCountryViewModel.Flag = urlPhoto;
                request.AddJsonBody(createCountryViewModel);
                var response = client.Post<CreateCountryViewModel>(request);
                return Redirect("/country/index");
            }
            return BadRequest();
        }

        private string UploadPhotoFriend(IFormFile photo)
        {
            var reader = photo.OpenReadStream();
            var cloudStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=lucasbarrozoazureat;AccountKey=3bg3N7SI+xKddxb+sq2CwGqBXofiRneDKerUTmP4SBMmzYkdSCsPqKUAHUXHSlDJfgDvxPtKtiBN1GjLLZkcdg==;EndpointSuffix=core.windows.net");
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("photo-friends");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var cloudDestination = blob.Uri.ToString();
            return cloudDestination;
        }

        // GET: CountryController/Edit/5
        public ActionResult Edit(int id)
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5000/api/countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);

            return View(response.Data);
        }

        // POST: CountryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Country countryN)
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5000/api/countries/" + id, DataFormat.Json);
                request.AddJsonBody(countryN);
                var response = client.Put<Country>(request);

                return Redirect("/country/index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CountryController/Delete/5
        public ActionResult Delete(int id)
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5000/api/countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);

            return View(response.Data);
        }

        // POST: CountryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5000/api/countries" + id, DataFormat.Json);
                var response = client.Delete<Country>(request);

                return Redirect("/country");
            }
            catch
            {
                return View();
            }
        }
    }
}
