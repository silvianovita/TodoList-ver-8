using Data.Base;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class Transaction :BaseModel
    {
        public DateTimeOffset OrderDate { get; set; }

        public Transaction() { }
        public Transaction(TransactionVM transactionVM)
        {
            this.OrderDate = transactionVM.OrderDate;
            this.CreateDate = DateTimeOffset.Now;
            this.IsDelete = false;
        }

        public void Update(TransactionVM transactionVM)
        {
            this.OrderDate = transactionVM.OrderDate;
            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}