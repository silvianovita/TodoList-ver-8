using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Context;
using Data.Model;
using Data.Repositories;
using Data.Repositories.Interface;
using Data.ViewModel;

namespace API.Services
{
    public class SupplierServices : ISupplierServices
    {
        private ISupplierRepository _supplierRepository;
        // = new SupplierRepository();
        //MyContext myContext = new MyContext();


        public SupplierServices(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public int Create(SupplierVM supplierVM)
        {
            if (string.IsNullOrWhiteSpace(supplierVM.Name))
            {
                return 0;
            }
            else
            {
                return _supplierRepository.Create(supplierVM);
            }
        }

        public int Delete(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return 0;
            }
            else
            {
                return _supplierRepository.Delete(id);
            }
        }

        public IEnumerable<Supplier> Get()
        {
            return _supplierRepository.Get();
        }

        public IEnumerable<Supplier> Get(int id)
        {
            return _supplierRepository.Get(id);
        }

        public async Task<IEnumerable<Supplier>> GetDapper()
        {
            return await _supplierRepository.GetDapper();
        }

        public async Task<IEnumerable<dynamic>> GetDapper(int id)
        {
            return await _supplierRepository.GetDapper(id);
        }

        public async Task<SupplierVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            return await _supplierRepository.Paging(keyword, pageNumber, pageSize);
        }

        public int Update(int id, SupplierVM supplierVM)
        {

            if (string.IsNullOrWhiteSpace(id.ToString()) || string.IsNullOrWhiteSpace(supplierVM.Name))
            {
                return 0;
            }
            else
            {
                return _supplierRepository.Update(id, supplierVM);
            }
        }
    }
}
