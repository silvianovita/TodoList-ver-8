using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Services.Interface
{
    public interface IItemServices
    {
        Task<IEnumerable<ItemVM>> Get();
        //IEnumerable<ItemVM> Get();
        Task<IEnumerable<ItemVM>> Get(int Id);
        Task<ItemVM> Paging(string keyword, int pageNumber, int pageSize);

        int Create(ItemVM itemVM);
        int Update(int id, ItemVM itemVM);
        int Delete(int id);
    }
}
