using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IItemService
    {
        Task<ItemResult> Get(int id);
        Task<IList<ItemResult>> Find();
        Task<ItemResult> Create(ItemRequest item);
        Task<ItemResult> Update(int id, ItemRequest item);
        Task Delete(int id);
    }
}
