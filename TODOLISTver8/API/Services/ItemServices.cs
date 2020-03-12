using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;

namespace API.Services
{
    public class ItemServices : IItemServices
    {
        private IItemRepository _itemRepository;
        public ItemServices(IItemRepository itemrepository)
        {
            _itemRepository = itemrepository;
        }

        public int Create(ItemVM itemVM)
        {
            _itemRepository.Create(itemVM);
            return 0;
        }

        public int Delete(int id)
        {
           return _itemRepository.Delete(id);
        }

        public async Task<IEnumerable<ItemVM>> Get()
        {
            return await _itemRepository.Get();
        }

        public async Task<IEnumerable<ItemVM>> Get(int Id)
        {
            return await _itemRepository.Get(Id);
        }

        public async Task<ItemVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            //return await _itemRepository.Paging(keyword, pageNumber, pageSize);
            return await _itemRepository.Paging(keyword, pageNumber, pageSize);
        }

        public int Update(int id, ItemVM itemVM)
        {
            return _itemRepository.Update(id, itemVM);
        }
    }
}
