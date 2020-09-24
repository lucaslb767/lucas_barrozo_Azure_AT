using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WebAzure.Controllers
{
    public class BrotherController : Controller
    {
        // GET: BrotherController
        public ActionResult Index()
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5003/api/Brothers/", DataFormat.Json);
            var response = client.Get<List<Brother>>(request);

            return View(response.Data);
        }

        // GET: BrotherController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrotherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrotherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BrotherResponse brother)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient();
                var request = new RestRequest("http://localhost:5003/api/brothers", DataFormat.Json);
                request.AddJsonBody(brother);
                var response = client.Post<Brother>(request);

                return Redirect("/friend/index");
            }
            return BadRequest();
        }

        // GET: BrotherController/Edit/5
        public ActionResult Edit(int id)
        {
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5003/api/Brothers/" + id, DataFormat.Json);
            var response = client.Get<Brother>(request);

            return View(response.Data);
        }

        // POST: BrotherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Brother brotherN)
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest("http://localhost:5003/api/Brothers/" + id, DataFormat.Json);
                request.AddJsonBody(brotherN);
                var response = client.Put<Brother>(request);

                return Redirect("/friend/index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BrotherController/Delete/5
        public ActionResult Delete(int id)
        {
            var client = new RestClient();

            var request = new RestRequest("http://localhost:5003/api/Brothers/" + id, DataFormat.Json);
            var response = client.Get<Brother>(request);

            return View(response.Data);
        }

        // POST: BrotherController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest("http://localhost:5003/api/Brothers/" + id, DataFormat.Json);
                var response = client.Delete<Brother>(request);

                return Redirect("/friend");
            }
            catch
            {
                return View();
            }
        }
    }
}
