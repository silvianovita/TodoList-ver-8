using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Outlook = Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;  
using iTextSharp.text.pdf;  
using System.IO;  
using iTextSharp.text;  
using System.Diagnostics;
using Client.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Client.Controllers
{
    
    public class UserController : Controller
    {
        readonly HttpClient Client = new HttpClient();
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            Client.BaseAddress = new Uri("https://localhost:44319/api/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetString("Id");
            if (id != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));

        }
        public ActionResult Login()
        {
            var id = HttpContext.Session.GetString("Id");
            if (id == null)
            {
                return View();
            }
            //return RedirectToAction("Index", "ToDoLists", new { area = "ToDoLists" });
            return RedirectToAction(nameof(Main));
        }
        public IActionResult First()
        {
            var id = HttpContext.Session.GetString("Id");
            if (id != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }
        //[Authorize(Roles = "Admin,User")]
        public IActionResult Main()
        {
            var id = HttpContext.Session.GetString("Id");
            if (id != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));

        }
        public ActionResult Register()
        {
            var a = HttpContext.Session.GetString("Id");
            var b = HttpContext.Session.GetString("UserName");
            //var c = HttpContext.Session.GetString("Token");
            if (a != null && b != null)
            {
                return RedirectToAction("Main");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                var hasil = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (hasil.Succeeded)
                {
                    // TODO: Add insert logic here
                    var myContent = JsonConvert.SerializeObject(loginVM);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = Client.PostAsync("Users/Login/", byteContent).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var user = result.Content.ReadAsAsync<TokenVM>().Result;
                        HttpContext.Session.SetString("Id", user.Id.ToString());
                        HttpContext.Session.SetString("Token", "Bearer " + user.AccessToken);
                        HttpContext.Session.SetString("Expire", user.ExpireToken.ToString());
                        HttpContext.Session.SetString("ExpireRefreshToken", user.ExpireRefreshToken.ToString());
                        HttpContext.Session.SetString("RefreshToken", user.RefreshToken);
                        HttpContext.Session.SetString("UserName", user.Username);
                        //var user = result.Content.ReadAsStringAsync().Result.Replace("\"", "").Split("...");
                        //HttpContext.Session.SetString("Token", "Bearer " + user[0]);
                        ////HttpContext.Session.SetString("Id", userVM.Id);
                        //HttpContext.Session.SetString("Id", user[1]);
                        //HttpContext.Session.SetString("UserName", user[2]);
                        Client.DefaultRequestHeaders.Add("Authorization", user.AccessToken);
                        //var data = result.Content.ReadAsAsync<User>();
                        //data.Wait();
                        //var user = data.Result;
                        //HttpContext.Session.SetString("Id", user.Id.ToString());
                        return RedirectToAction(nameof(Main));
                        //return RedirectToAction("Main");
                    }
                }
                return View();
            }
            catch(Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Register(UserVM userVM)
        {
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PostAsync("users/register/", byteContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Main");
            }
            return View();
        }

        public async Task<IEnumerable<ToDoListVM>> Search(string keyword, int status)
        {
            try
            {
                Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                var response = await Client.GetAsync("ToDoLists/Search/" + HttpContext.Session.GetString("Id") + "/" + keyword + "/" + status);
                if (response.IsSuccessStatusCode)
                {
                    var e = await response.Content.ReadAsAsync<List<ToDoListVM>>();
                    return e;
                }
            }
            catch (Exception m)
            {

            }
            return null;
        }

        public async Task RefToken(TokenVM tokenVM)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                var myContent = JsonConvert.SerializeObject(tokenVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = Client.PostAsync("Users/refresh", byteContent).Result;

                if (result.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                {
                    var tes = await result.Content.ReadAsAsync<TokenVM>();
                    var token2 = tes.AccessToken;
                    var exptoken2 = tes.ExpireToken;
                    var reftoken2 = tes.RefreshToken;
                    var expreftoken2 = tes.ExpireRefreshToken;
                    HttpContext.Session.SetString("Token", token2);
                    HttpContext.Session.SetString("Expire", exptoken2.ToString());
                    HttpContext.Session.SetString("ExpireRefreshToken", expreftoken2.ToString());
                    HttpContext.Session.SetString("RefreshToken", reftoken2);
                }

        }
        [Authorize(Roles = "Admin, User")]
        public async Task<ToDoListVM> Paging(int pageSize, int pageNumber, int status, string keyword)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));

            TokenVM tokenVM = new TokenVM();

            var username = HttpContext.Session.GetString("UserName");
            var exptoken = Convert.ToInt64(HttpContext.Session.GetString("Expire"));
            var reftoken = HttpContext.Session.GetString("RefreshToken");
            var expreftoken = Convert.ToInt64(HttpContext.Session.GetString("ExpireRefreshToken"));

            #region Bisa
            if (exptoken < DateTime.UtcNow.Ticks && expreftoken > DateTime.UtcNow.Ticks)
            {
                await RefToken(tokenVM);
                #region bisa
                //var myContent = JsonConvert.SerializeObject(reftoken);
                //var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var result = Client.PostAsync("Users/refresh", byteContent).Result;

                //if (result.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                //{
                //    var tes = await result.Content.ReadAsAsync<TokenVM>();
                //    var token2 = tes.AccessToken;
                //    var exptoken2 = tes.ExpireToken;
                //    var reftoken2 = tes.RefreshToken;
                //    var expreftoken2 = tes.ExpireRefreshToken;
                //    HttpContext.Session.SetString("Token", token2);
                //    HttpContext.Session.SetString("Expire", exptoken2.ToString());
                //    HttpContext.Session.SetString("ExpireRefreshToken", expreftoken2.ToString());
                //    HttpContext.Session.SetString("RefreshToken", reftoken2);
                //}
                #endregion
            }
            else if (expreftoken < DateTime.UtcNow.Ticks)
            {
                return null;
            }
            #endregion
            #region paging biasa
            try
            {
                //Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                //var response = await Client.GetAsync("ToDoLists/paging?UserId=" + HttpContext.Session.GetString("UserName") + "&status=" + status + "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                var response = await Client.GetAsync("ToDoLists/paging?UserId=" + username + "&status=" + status + "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                var testing = response;
                if (response.IsSuccessStatusCode)
                {
                    var e = await response.Content.ReadAsAsync<ToDoListVM>();
                    return e;
                }
            }
            catch (Exception m)
            {

            }
            return null;
            #endregion
        }

        public async Task<IList<ToDoListVM>> List(int status)
        {
            string Id = HttpContext.Session.GetString("Id");
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            IEnumerable<ToDoListVM> users = null;
            var responseTask = await Client.GetAsync("ToDoLists/GetTodoLists/" + Id + '/' + status);
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsAsync<IList<ToDoListVM>>();
                return readTask;
                //return Ok(new { data = readTask });
            }
            return null;
        }
        public async Task<IActionResult> Excel()
        {
            int status = 3;
            string Id = HttpContext.Session.GetString("Id");
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            IEnumerable<ToDoListVM> users = null;
            var responseTask = await Client.GetAsync("ToDoLists/GetTodoLists/" + Id + '/' + status);
            var readTask = await responseTask.Content.ReadAsAsync<IList<ToDoListVM>>();

            var comlumHeadrs = new string[]
            {
                "Id",
                "Name",
                "Status"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add("Current ToDoList"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }
                //Add values
                var j = 2;
                foreach (var supp in readTask)
                {
                    int no = 1;
                    for (int i = 0; i < readTask.Count; i++)
                    {
                        worksheet.Cells["A" + j].Value = i;
                        i++;
                    }

                    worksheet.Cells["B" + j].Value = supp.Name;
                    worksheet.Cells["C" + j].Value = supp.Status;

                    j++;
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"ToDoList Table.xlsx");
        }
        public async Task<IActionResult> ExcelItem()
        {
            var comlumHeadrsItem = new string[]
           {
                "Id",
                "Name",
                "Stock",
                "Price",
                "Supplier Name"
           };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var ItemResponse = await Client.GetAsync("Items");
                var ItemTask = await ItemResponse.Content.ReadAsAsync<IList<ItemVM>>();

                // add a new worksheet to the empty workbook
                var Itm = package.Workbook.Worksheets.Add("Current Item"); //Worksheet name
                using (var cells = Itm.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrsItem.Count(); i++)
                {
                    Itm.Cells[1, i + 1].Value = comlumHeadrsItem[i];
                }
                //Add values
                var k = 2;
                foreach (var items in ItemTask)
                {
                    Itm.Cells["A" + k].Value = items.Id;
                    Itm.Cells["B" + k].Value = items.Name;
                    Itm.Cells["C" + k].Value = items.Stock;
                    Itm.Cells["D" + k].Value = items.Price;
                    Itm.Cells["E" + k].Value = items.SupplierName;

                    k++;
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Items.xlsx");
        }

        [HttpGet("User/PageData/{status}")]
        public IActionResult PageData(int status, IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = request.Search.Value;
            //var data = Search(keyword, status).Result;
            //var filteredData = data;
            var dataPage = Paging(pageSize, pageNumber, status, keyword).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filterLength, dataPage.data);

            return new DataTablesJsonResult(response, true);
            //var data = List(status).Result;

            //var filteredData = String.IsNullOrWhiteSpace(request.Search.Value)
            //    ? data
            //    : data.Where(_item => _item.Name.Contains(request.Search.Value));

            //var dataPage = filteredData.Skip(request.Start).Take(request.Length);

            //var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            //return new DataTablesJsonResult(response, true);
        }

        public JsonResult Insert(ToDoListVM todolistVM)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            todolistVM.userId = HttpContext.Session.GetString("Id");
            var myContent = JsonConvert.SerializeObject(todolistVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PostAsync("todolists", byteContent).Result;
            return Json(result);
        }
        public JsonResult Update(int id, ToDoListVM grade)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var myContent = JsonConvert.SerializeObject(grade);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PutAsync("todolists/" + id, byteContent).Result;
            return Json(result);

        }
        //[HttpGet("{Id}")]
        public async Task<JsonResult> GetbyIDAsync(int id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            HttpResponseMessage response = await Client.GetAsync("todolists");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<ToDoList>>();
                var attitude = data.FirstOrDefault(s => s.Id == id);
                var json = JsonConvert.SerializeObject(attitude, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return Json(json);
            }
            return Json("Internal Server Error");
        }
        public JsonResult UpdateCheckedTodoList(int Id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = Client.DeleteAsync("todolists/updateCheckedTodolist/" + Id).Result;
            //if (result != null)
            //{
            //    //Send Outlook 
            //    try
            //    {
            //        Outlook._Application _app = new Outlook.Application();
            //        Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
            //        mail.To = "silvianovita678@gmail.com";
            //        mail.Subject = "Your Data has been saved";
            //        mail.Body = "Hi , this email is automatically sent to inform you, that your data (included this email) has been saved in Bootcamp32 ";
            //        mail.Importance = Outlook.OlImportance.olImportanceNormal;
            //        ((Outlook._MailItem)mail).Send();
                    
            //    }
            //    catch (Exception ex)
            //    {
                   
            //    }
            //}
            return Json(result);
        }
        public JsonResult UpdateUnCheckedTodoList(int Id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var myContent = JsonConvert.SerializeObject(Id);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PutAsync("toDoLists/updateUncheckedTodolist/" + Id, byteContent).Result;
            return Json(result);
        }
        public JsonResult Delete(int id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = Client.DeleteAsync("ToDoLists/" + id).Result;
            return Json(result);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = Client.GetAsync("users/Logout").Result;
            if (result.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("Id");
                HttpContext.Session.Remove("UserName");
                HttpContext.Session.Remove("Token");
                HttpContext.Session.Remove("Expire");
                HttpContext.Session.Remove("ExpireRefreshToken");
                HttpContext.Session.Remove("RefreshToken");


                HttpContext.Session.Clear();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public ActionResult Report(ToDoListVM toDoListVM)
        {
            ToDoListReport _toDoListReport = new ToDoListReport();
            byte[] abytes = _toDoListReport.PrepareReport(GetToDoList());
            return File(abytes, "application/pdf");
        }

        public List<ToDoListVM> GetToDoList()
        {
            List<ToDoListVM> todolistVMs= new List<ToDoListVM>();
            ToDoListVM toDoListVM = new ToDoListVM();
            //for (int i = 0; i < todolistVMs.Count; i++)
            for (int i = 0; i < 10; i++)
            {
                toDoListVM = new ToDoListVM();
                toDoListVM.Id = i;
                toDoListVM.Name = "Name";
                todolistVMs.Add(toDoListVM);
            }
            return todolistVMs;
        }
    }
}
    