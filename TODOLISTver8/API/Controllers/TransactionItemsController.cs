using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionItemsController : ControllerBase
    {
        ITransactionItemServices _transactionItemServices;
        public TransactionItemsController(ITransactionItemServices transactionItemServices)
        {
            _transactionItemServices = transactionItemServices;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_transactionItemServices.Get());
        }
        [HttpPost]
        public IActionResult Create(TransactionItemVM transactionItemVM)
        {
            return Ok(_transactionItemServices.Create(transactionItemVM));
        }

        [HttpGet]
        [Route("Paging")]
        public async Task<TransactionItemVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            return await _transactionItemServices.Paging(keyword, pageNumber, pageSize);
        }
    }
}