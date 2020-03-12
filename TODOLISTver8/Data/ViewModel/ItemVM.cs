using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ItemVM
    {
        public IEnumerable<ItemVM> data { get; set; }
        public int filterLength { get; set; }
        public int length { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
