using Data.Base;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class TransactionItem :BaseModel
    {
        public int Quantity { get; set; }
        public Transaction transaction { get; set; }
        public Item item { get; set; }
        public TransactionItem() { }
        public TransactionItem(TransactionItemVM transactionItemVM)
        {
            this.Quantity = transactionItemVM.Quantity;
        }
        public void Update(TransactionItemVM transactionItemVM)
        {
            this.Quantity = transactionItemVM.Quantity;
            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}

