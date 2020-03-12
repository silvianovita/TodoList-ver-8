using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace Data.Repositories.Interface
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemVM>> Get();
        //IEnumerable<ItemVM> Get();
        Task<IEnumerable<ItemVM>> GetTodoLists(string userId, int status);
        Task<IEnumerable<ItemVM>> Get(int Id);

        IEnumerable<ItemVM> Search(string userId, string keyword, int param1);
        //IEnumerable<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize);
        Task<ItemVM> Paging(string keyword, int pageNumber, int pageSize);

        int Create(ItemVM itemVM);
        int Update(int Id, ItemVM itemVM);
        int Delete(int Id);
    }
}
