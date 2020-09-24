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
using WebAzure.Models.FriendModel;

namespace WebAzure.Controllers
{
    public class FriendController : Controller
    {
        // GET: FriendController
        public ActionResult Index()
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5003/api/friends", DataFormat.Json);
            var response = client.Get<List<Friend>>(request);

            return View(response.Data);
        }

        // GET: FriendController/Details/5
        public ActionResult Details(int id)
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5003/api/friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);
            return View(response.Data);
        }

        // GET: FriendController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FriendController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateFriendViewModel createFriendViewModel)
        {

            if (ModelState.IsValid)
            {
                var client = new RestClient();
                var request = new RestRequest("http://localhost:5003/api/friends", DataFormat.Json);
                var urlPhoto = UploadPhotoFriend(createFriendViewModel.PhotoFile);
                createFriendViewModel.Photo = urlPhoto;
                request.AddJsonBody(createFriendViewModel);
                var response = client.Post<CreateFriendViewModel>(request);
                return Redirect("/friend/index");
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

        // GET: FriendController/Edit/5
        public ActionResult Edit(int id)
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5003/api/friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);
            return View(response.Data);
        }

        // POST: FriendController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Friend friendN)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest("http://localhost:5003/api/friends/" + id, DataFormat.Json);
                request.AddJsonBody(friendN);

                return Redirect("/friend/index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendController/Delete/5
        public ActionResult Delete(int id)
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5003/api/friends/" + id, DataFormat.Json);
            var response = client.Get<Friend>(request);

            return View(response.Data);
        }

        // POST: FriendController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest("http://localhost:5003/api/friends/" + id, DataFormat.Json);
                var response = client.Delete<Friend>(request);

                return Redirect("/friend");
            }
            catch
            {
                return View();
            }
        }
    }
}
