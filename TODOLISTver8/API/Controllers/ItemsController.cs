using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IItemServices _itemservice;// = new SupplierServices();
        public ItemsController(IItemServices itemServices)
        {
            _itemservice = itemServices;
        }
        [HttpGet]
        //public IActionResult Get()
        public async Task<IEnumerable<ItemVM>> Get()
        {
            return await _itemservice.Get();
        }
        [HttpGet("{Id}")]
        public async Task<IEnumerable<ItemVM>> Get(int Id)
        {
            return await _itemservice.Get(Id);
        }
        
        [HttpPost]
        public IActionResult Create(ItemVM itemVM)
        {
            return Ok(_itemservice.Create(itemVM));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ItemVM itemVM)
        {
            return Ok(_itemservice.Update(id, itemVM));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            return Ok(_itemservice.Delete(id));
        }
        [HttpGet]
        [Route("Paging")]
        //public IEnumerable<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize)
        public async Task<ItemVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            //keyword = keyword.Substring(3);
            if (keyword == null)
            {
                keyword = "";
            }
            return await _itemservice.Paging(keyword, pageNumber, pageSize);
        }
    }
}