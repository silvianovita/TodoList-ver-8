using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class TransactionItemVM
    {
        public IEnumerable<TransactionItemVM> data { get; set; }
        public int filterLength { get; set; }
        public int length { get; set; }

        public int Id { get; set; }
        public int Quantity { get; set; }
        public int itemId { get; set; }
        public int transactionId { get; set; }

        public int supplierId { get; set; }
    }
}
