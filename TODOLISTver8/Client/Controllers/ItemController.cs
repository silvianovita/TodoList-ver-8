using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ItemController : Controller
    {
        readonly HttpClient Client = new HttpClient();
        public ItemController()
        {
            Client.BaseAddress = new Uri("https://localhost:44319/api/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetbyID(int Id)
        {
            IEnumerable<ItemVM> item = null;
            var responseTask = Client.GetAsync("items/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<ItemVM>>();
                readTask.Wait();
                item = readTask.Result;
            }
            else
            {
                item = Enumerable.Empty<ItemVM>();
                ModelState.AddModelError(string.Empty, "Server error try after some time");
            }
            return Json(item);
        }

        // GET: Employees/Create
        public JsonResult Insert(ItemVM itemVM)
        {
            var myContent = JsonConvert.SerializeObject(itemVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PostAsync("items", byteContent).Result;
            return Json(result);
        }
        public JsonResult Update(int id, ItemVM itemVM)
        {
            var myContent = JsonConvert.SerializeObject(itemVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PutAsync("items/" + id, byteContent).Result;
            return Json(result);

        }
        public JsonResult Delete(int id)
        {
            var result = Client.DeleteAsync("items/" + id).Result;
            return Json(result);
        }
    }
}   