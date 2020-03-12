using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;

namespace API.Services
{
    public class TransactionItemServices : ITransactionItemServices
    {
        ITransactionItemRepository _transactionRepository;
        public TransactionItemServices(ITransactionItemRepository transactionItemRepository)
        {
            _transactionRepository = transactionItemRepository;
        }

        public int Create(TransactionItemVM transactionItemVM)
        {
            return _transactionRepository.Create(transactionItemVM);
        }

        public int Delete(int id)
        {
            return _transactionRepository.Delete(id);
        }

        public async Task<IEnumerable<TransactionItem>> Get()
        {
            return await _transactionRepository.Get();
        }

        public async Task<IEnumerable<TransactionItemVM>> Get(int Id)
        {
            return await _transactionRepository.Get(Id);
        }

        public async Task<TransactionItemVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            return await _transactionRepository.Paging(keyword, pageNumber, pageSize);
        }

        public int Update(int id, TransactionItemVM transactionItemVM)
        {
            return _transactionRepository.Update(id, transactionItemVM);
        }
    }
}
