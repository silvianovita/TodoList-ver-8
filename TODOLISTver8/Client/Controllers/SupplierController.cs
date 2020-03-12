using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Client.Report;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Client.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class SupplierController : Controller
    {
        readonly HttpClient Client = new HttpClient();
        public SupplierController()
        {
            Client.BaseAddress = new Uri("https://localhost:44319/api/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         }
        public IActionResult Index()
        {
            return View();
        }
        //[Authorize(Roles = "Role")]
        public async Task<IList<SupplierVM>> List()
        {
            IEnumerable<SupplierVM> users = null;
            var responseTask = await Client.GetAsync("Suppliers");
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsAsync<IList<SupplierVM>>();
                return readTask;
                //return Ok(new { data = readTask });
            }
            return null;
        }
        public async Task<IActionResult> Excel()
        {

            var responseTask = await Client.GetAsync("Suppliers");
            var readTask = await responseTask.Content.ReadAsAsync<IList<SupplierVM>>();


            var ItemResponse = await Client.GetAsync("Items");
            var ItemTask = await ItemResponse.Content.ReadAsAsync<IList<ItemVM>>();


            var comlumHeadrs = new string[]
            {
                "Id",
                "Name",
                "JoinDate"
            };
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
                // add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add("Current Supplier"); //Worksheet name
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
                    worksheet.Cells["A" + j].Value = supp.Id;
                    worksheet.Cells["B" + j].Value = supp.Name;
                    worksheet.Cells["C" + j].Value = supp.JoinDate.ToString("dd-MM-yyyy");

                    j++;
                }
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
            return File(result, "application/ms-excel", $"Master Transaction Table.xlsx");
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
            return File(result, "application/m6s-excel", $"Items.xlsx");
        }
        public async Task<IActionResult> CSV()
        {
            var responseTask = await Client.GetAsync("Suppliers");
            var readTask = await responseTask.Content.ReadAsAsync<IList<SupplierVM>>();

            var comlumHeadrs = new string[]
            {
                "Id",
                "Name",
                "Joined Date"
            };

            var employeeRecords = (from supp in readTask
                                   select new object[]
                                   {
                                            supp.Id,
                                            $"{supp.Name}",
                                            supp.JoinDate.ToString("MM/dd/yyyy"),
                                   }).ToList();

            // Build the file content
            var employeecsv = new StringBuilder();
            employeeRecords.ForEach(line =>
            {
                employeecsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{employeecsv.ToString()}");
            return File(buffer, "text/csv", $"Supplier.csv");

        }
        public async Task<IActionResult> CSVItem()
        {
            var responseTask = await Client.GetAsync("Items");
            var readTask = await responseTask.Content.ReadAsAsync<IList<ItemVM>>();

            var comlumHeadrs = new string[]
            {
                "Id",
                "Name",
                "Stock",
                "Price",
                "Supplier Name"
            };

            var employeeRecords = (from itm in readTask
                                   select new object[]
                                   {
                                            itm.Id,
                                            $"{itm.Name}",
                                            $"{itm.Stock}",
                                            $"{itm.Price}",
                                            itm.SupplierName
                                   }).ToList();

            // Build the file content
            var employeecsv = new StringBuilder();
            employeeRecords.ForEach(line =>
            {
                employeecsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{employeecsv.ToString()}");
            return File(buffer, "text/csv", $"Item.csv");

        }
        //public JsonResult List()
        //{
        //    IEnumerable<Supplier> suppliers = null;
        //    var responseTask = Client.GetAsync("suppliers");
        //    responseTask.Wait();
        //    var result = responseTask.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var readTask = await responseTask.Content.ReadAsAsync<IList<ToDoListVM>>();
        //        return readTask;
        //        //var readTask = result.Content.ReadAsAsync<IList<Supplier>>();
        //        readTask.Wait();
        //        suppliers = readTask.Result;
        //    }
        //    else
        //    {
        //        suppliers = Enumerable.Empty<Supplier>();
        //        ModelState.AddModelError(string.Empty, "Server error try after some time");
        //    }
        //    return Json(suppliers);
        //}
        public JsonResult GetbyID(int Id)
        {
            IEnumerable<Supplier> supplier = null;
            var responseTask = Client.GetAsync("suppliers/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<Supplier>>();
                readTask.Wait();
                supplier = readTask.Result;
            }
            else
            {
                supplier = Enumerable.Empty<Supplier>();
                ModelState.AddModelError(string.Empty, "Server error try after some time");
            }
            return Json(supplier);
        }

        // GET: Employees/Create
        public JsonResult Insert(SupplierVM supplierVM)
        {
            var myContent = JsonConvert.SerializeObject(supplierVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PostAsync("suppliers", byteContent).Result;
            return Json(result);
        }
        public JsonResult Update(int id, SupplierVM supplierVM)
        {
            var myContent = JsonConvert.SerializeObject(supplierVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PutAsync("suppliers/"+id, byteContent).Result;
            return Json(result);

        }
        public JsonResult Delete(int id)
        {
            var result = Client.DeleteAsync("suppliers/" + id).Result;
            return Json(result);
        }
        public async Task<SupplierVM> Paging(int pageSize, int pageNumber, string keyword)
        {
            try
            {
                Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                //var response = await Client.GetAsync("Suppliers/paging?UserId=" + HttpContext.Session.GetString("UserName") +  "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                var response = await Client.GetAsync("Suppliers/paging?keyword=" + keyword  + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize);
                var testing = response;
                //var response = await Client.GetAsync("ToDoLists/Paging/" + HttpContext.Session.GetString("Id") + "/" +status+"/"+ keyword+"/" + pageNumber + "/" + pageSize);
                if (response.IsSuccessStatusCode)
                {
                    //var e = await response.Content.ReadAsAsync<ToDoListVM>();
                    var e = await response.Content.ReadAsAsync<SupplierVM>();
                    return e;
                }
            }
            catch (Exception m)
            {

            }
            return null;
        }
        public async Task<ItemVM> PagingItem(int pageSize, int pageNumber, string keyword)
        {
            try
            {
                Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                //var response = await Client.GetAsync("Suppliers/paging?UserId=" + HttpContext.Session.GetString("UserName") +  "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                var response = await Client.GetAsync("Items/paging?keyword=" + keyword + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize);
                var testing = response;
                //var response = await Client.GetAsync("ToDoLists/Paging/" + HttpContext.Session.GetString("Id") + "/" +status+"/"+ keyword+"/" + pageNumber + "/" + pageSize);
                if (response.IsSuccessStatusCode)
                {
                    //var e = await response.Content.ReadAsAsync<ToDoListVM>();
                    var e = await response.Content.ReadAsAsync<ItemVM>();
                    return e;
                }
            }
            catch (Exception m)
            {

            }
            return null;
        }
        [HttpGet("Supplier/PageData")]
        public IActionResult PageData(IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = request.Search.Value;
            var dataPage = Paging(pageSize, pageNumber, keyword).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filterLength, dataPage.data);

            return new DataTablesJsonResult(response, true);
        }
        [HttpGet("Supplier/PageDataItem")]
        public IActionResult PageDataItem(IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = request.Search.Value;
            var dataPage = PagingItem(pageSize, pageNumber, keyword).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filterLength, dataPage.data);

            return new DataTablesJsonResult(response, true);
        }
        public ActionResult Report(SupplierVM supplierVM)
        {
            SupplierReport _supplierReport = new SupplierReport();
            byte[] abytes =  _supplierReport.PrepareReport();
            return File(abytes, "application/pdf");
        }

        public async Task<List<SupplierVM>> GetSupplier()
        {
            var responseTask = await Client.GetAsync("Suppliers");
            var readTask = await responseTask.Content.ReadAsAsync<IList<SupplierVM>>();

            List<SupplierVM> supplierVMs = new List<SupplierVM>();
            SupplierVM supplierVM = new SupplierVM();
            var data = (List<SupplierVM>) null;
            //for (int i = 0; i < supplierVMs.Count; i++)
            for (int i = 0; i < readTask.Count; i++)
            {
                data = readTask.ToList();
                //supplierVM.Id = i;
                //supplierVM.Name = data.Name
                //supplierVM = new SupplierVM();
                //supplierVM.Id = i;
                //supplierVM.Name = "Name";
                //supplierVM.Add(supplierVM);
            }
            return supplierVMs;
        }
    }
}