using System;
using System.Collections.Generic;
using System.Text;
using Data.Model;
using Data.ViewModel;

namespace Data.Repositories.Interface
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> Get();
        IEnumerable<Transaction> Get(int id);
        int Create(TransactionVM transactionVM);
        int Update(int id, TransactionVM transactionVM);
        int Delete(int id);
    }
}
