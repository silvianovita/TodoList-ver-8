using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Services.Interface
{
    public interface ITransactionServices
    {
        IEnumerable<Transaction> Get();
        IEnumerable<Transaction> Get(int id);

        int Create(TransactionVM transactionVM);
        int Update(int id, TransactionVM transactionVM);
        int Delete(int id);
    }
}
