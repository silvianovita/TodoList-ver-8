using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Services.Interface
{
    public interface ITransactionItemServices
    {
        Task<IEnumerable<TransactionItem>> Get();
        Task<IEnumerable<TransactionItemVM>> Get(int Id);
        Task<TransactionItemVM> Paging(string keyword, int pageNumber, int pageSize);

        int Create(TransactionItemVM transactionItemVM);
        int Update(int id, TransactionItemVM transactionItemVM);
        int Delete(int id);

    }
}
