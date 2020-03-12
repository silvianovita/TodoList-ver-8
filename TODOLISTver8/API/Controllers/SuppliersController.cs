using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using API.Services.Interface;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private ISupplierServices _supplierservice;
        public SuppliersController(ISupplierServices supplierServices)
        {
            _supplierservice = supplierServices;
        }
        //[HttpGet]
        //public async Task<IEnumerable<Supplier>> Get()
        //{
        //    return await _supplierservice.GetDapper();
        //}
        [HttpGet]
        public IEnumerable<Supplier> Get()
        {
            return  _supplierservice.Get();
        }


        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    return Ok(_supplierservice.Get(id) ?? null);
        //}

        [HttpGet("{id}")]
        public async Task<IEnumerable<dynamic>> Get(int id)
        {
            return await _supplierservice.GetDapper(id);
        }

        [HttpPost]
        public IActionResult Post(SupplierVM supplierVM)
        {
            if (ModelState.IsValid)
            {
                var push = _supplierservice.Create(supplierVM);
                //if (push != 0)
                //{ 
                //    return Ok();
                //}
            }
            return Ok();
            //return StatusCode(500);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, SupplierVM supplierVM)
        {
            if (ModelState.IsValid)
            {
                var push = _supplierservice.Update(id, supplierVM);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var push = _supplierservice.Delete(id);
            }
            return Ok();
        }
        [HttpGet]
        [Route("Paging")]
        //public IEnumerable<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize)
        public async Task<SupplierVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            //keyword = keyword.Substring(3);
            if (keyword == null)
            {
                keyword = "";
            }
            return await _supplierservice.Paging(keyword, pageNumber, pageSize);
        }
    }
}