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
    public class TransactionsController : ControllerBase
    {
        ITransactionServices _transactionServices;
        
        public TransactionsController(ITransactionServices transactionServices)
        {
            _transactionServices = transactionServices;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_transactionServices.Get());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_transactionServices.Get(id));
        }
        [HttpPost]
        public IActionResult Post(TransactionVM transactionVM)
        {
            return Ok(_transactionServices.Create(transactionVM));
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, TransactionVM transactionVM)
        {
            return Ok(_transactionServices.Update(id, transactionVM));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return Ok(_transactionServices.Delete(id));
        }
    }
}