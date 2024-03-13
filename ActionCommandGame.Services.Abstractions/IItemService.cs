using System.Collections.Generic;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IItemService
    {
        Item Get(int id);
        IList<Item> Find();
        Item Create(Item item);
        Item Update(int id, Item item);
        bool Delete(int id);
    }
}
