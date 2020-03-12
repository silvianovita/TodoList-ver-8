using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class SupplierVM
    {
        public IEnumerable<SupplierVM> data { get; set; }
        public int filterLength { get; set; }
        public int length { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset JoinDate { get; set; }

        public void Add(SupplierVM supplierVM)
        {
            throw new NotImplementedException();
        }
    }
}
