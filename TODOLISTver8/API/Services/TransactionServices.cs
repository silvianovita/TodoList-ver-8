using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;

namespace API.Services
{
    public class TransactionServices : ITransactionServices
    {
        ITransactionRepository _transactionRepository;
        public TransactionServices(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public int Create(TransactionVM transactionVM)
        {
            return _transactionRepository.Create(transactionVM);
        }

        public int Delete(int id)
        {
            return _transactionRepository.Delete(id);
        }
        

        public IEnumerable<Transaction> Get()
        {
            return _transactionRepository.Get();
        }

        public IEnumerable<Transaction> Get(int id)
        {
            return _transactionRepository.Get(id);
        }

        public int Update(int id, TransactionVM transactionVM)
        {
            return _transactionRepository.Update(id, transactionVM);
        }
    }
}
