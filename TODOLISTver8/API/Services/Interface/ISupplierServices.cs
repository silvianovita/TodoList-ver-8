using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Services.Interface
{
    public interface ISupplierServices
    {
        IEnumerable<Supplier> Get();
        IEnumerable<Supplier> Get(int id);

        Task<IEnumerable<Supplier>> GetDapper();
        Task<IEnumerable<dynamic>> GetDapper(int id);
        Task<SupplierVM> Paging(string keyword, int pageNumber, int pageSize);

        int Create(SupplierVM supplierVM);
        int Update(int id, SupplierVM supplierVM);
        int Delete(int id);
    }
}
