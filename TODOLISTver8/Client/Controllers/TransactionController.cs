using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    [Authorize(Roles = "User")]
    public class TransactionController : Controller
    {
        readonly HttpClient Client = new HttpClient();
        public TransactionController()
        {
            Client.BaseAddress = new Uri("https://localhost:44319/api/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult Insert(TransactionItemVM transactionItemVM)
        {
            var myContent = JsonConvert.SerializeObject(transactionItemVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PostAsync("transactionItems", byteContent).Result;
            return Json(result);
        }
        public async Task<TransactionItemVM> Paging(int pageSize, int pageNumber, string keyword)
        {
            try
            {
                Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                //var response = await Client.GetAsync("Suppliers/paging?UserId=" + HttpContext.Session.GetString("UserName") +  "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                var response = await Client.GetAsync("TransactionItems/paging?keyword=" + keyword + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize);
                var testing = response;
                //var response = await Client.GetAsync("ToDoLists/Paging/" + HttpContext.Session.GetString("Id") + "/" +status+"/"+ keyword+"/" + pageNumber + "/" + pageSize);
                if (response.IsSuccessStatusCode)
                {
                    //var e = await response.Content.ReadAsAsync<ToDoListVM>();
                    var e = await response.Content.ReadAsAsync<TransactionItemVM>();
                    return e;
                }
            }
            catch (Exception m)
            {

            }
            return null;
        }
        [HttpGet("Transaction/PageData")]
        public IActionResult PageData(IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = request.Search.Value;
            var dataPage = Paging(pageSize, pageNumber, keyword).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filterLength, dataPage.data);

            return new DataTablesJsonResult(response, true);
        }
    }
}