using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        MyContext myContext = new MyContext();
        public int Create(TransactionVM transactionVM)
        {
            var OrderDate = new SqlParameter("@OrderDate",transactionVM.OrderDate);
            return myContext.Database.ExecuteSqlRaw("sp_insertdataTransaction @OrderDate",OrderDate);
        }

        public int Delete(int id)
        {
            var Id = new SqlParameter("@Id", id);
            return myContext.Database.ExecuteSqlRaw("sp_deletedataTransaction @Id", Id);
        }

        public IEnumerable<Transaction> Get()
        {
            var result = myContext.Transactions.FromSqlRaw($"sp_getTransactiondelisfalse");
            return result;
        }

        public IEnumerable<Transaction> Get(int id)
        {
            var Id = new SqlParameter("@Id", id);
            var result = myContext.Transactions.FromSqlRaw($"sp_GetbyIdTransaction @Id",id);
            return result;
        }

        public int Update(int id, TransactionVM transactionVM)
        {
            var Id = new SqlParameter("@Id", id);
            var orderdate = new SqlParameter("@OrderDate", transactionVM.OrderDate);
            return myContext.Database.ExecuteSqlRaw("sp_UpdateDataTransaction @Id, @OrderDate",Id,orderdate);
        }
    }
}
