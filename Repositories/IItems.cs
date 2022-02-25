using System;
using API.Models;

namespace API.Repositories
{
	public interface IItems
		{
			public IEnumerable<Item> GetItems();

			public Item GetItemById(Guid id);

			public void CreateItem(Item item);

			public void UpdateItem(Guid id, Item item);

			public void DeleteItem(Guid id);
		}
}

