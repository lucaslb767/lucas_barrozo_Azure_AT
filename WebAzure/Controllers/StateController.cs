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
using WebAzure.Models.StateModel;

namespace WebAzure.Controllers
{
    public class StateController : Controller
    {
        // GET: StateController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5000/api/states", DataFormat.Json);
            var response = client.Get<List<State>>(request);

            return View(response.Data);
        }

        // GET: StateController/Details/5
        public ActionResult Details(int id)
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5000/api/states/" + id, DataFormat.Json);
            var response = client.Get<State>(request);
            return View(response.Data);
        }

        // GET: StateController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateStateViewModel createStateViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5000/api/states", DataFormat.Json);
                var urlPhoto = UploadPhotoFriend(createStateViewModel.PhotoFile);
                createStateViewModel.Flag = urlPhoto;
                request.AddJsonBody(createStateViewModel);
                var response = client.Post<CreateStateViewModel>(request);
                return Redirect("/state/index");
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

        // GET: StateController/Edit/5
        public ActionResult Edit(int id)
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5000/api/states/" + id, DataFormat.Json);
            var response = client.Get<State>(request);

            return View(response.Data);
        }

        // POST: StateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, State stateN)
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5000/api/states/" + id, DataFormat.Json);
                request.AddJsonBody(stateN);
                var response = client.Put<State>(request);

                return Redirect("/state/index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StateController/Delete/5
        public ActionResult Delete(int id)
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5000/api/states/" + id, DataFormat.Json);
            var response = client.Get<State>(request);
            return View(response.Data);
        }

        // POST: StateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5000/api/states/" + id, DataFormat.Json);
                var response = client.Delete<State>(request);

                return Redirect("/state");
            }
            catch
            {
                return View();
            }
        }
    }
}
