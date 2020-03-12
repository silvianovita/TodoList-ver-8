using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace Data.Repositories.Interface
{
    public interface ITransactionItemRepository
    {

        Task<IEnumerable<TransactionItem>> Get();
        Task<IEnumerable<TransactionItemVM>> GetTodoLists(string userId, int status);
        Task<IEnumerable<TransactionItemVM>> Get(int Id);

        IEnumerable<TransactionItemVM> Search(string userId, string keyword, int param1);
        Task<TransactionItemVM> Paging(string keyword, int pageNumber, int pageSize);

        int Create(TransactionItemVM transactionItemVM);
        int Update(int Id, TransactionItemVM transactionItemVM);
        int Delete(int Id);
    }
}
