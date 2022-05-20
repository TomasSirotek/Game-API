// using System;
// using System.Net;
// using API.Models;
//
// namespace API.Repositories
// {
// 	public class InMemItemRepo: IItems
// 	{
// 		private List<Item> _Items;
//
// 		public InMemItemRepo() 
// 		{
// 			_Items = new () {
// 				new Item {
// 					Id = Guid.NewGuid(),
// 					Title = "Item 1",
// 					Price = 200,
// 					Note = "sss"
// 				}
// 		};
// 	}
// 	public void CreateItem (Item item)
// 	{
// 		_Items.Add(item);
// 	}
//
// 	public Item GetItemById(Guid id)
// 	{
// 		var item = _Items.Where(x => x.Id == id).SingleOrDefault();
// 		return item;
// 	}
// 	public IEnumerable<Item> GetItems()
// 	{
// 		return _Items;
// 	}
// 	public void DeleteItem(Guid id)
// 	{
// 		var itemIndex = _Items.FindIndex(x => x.Id == id);
// 		if (itemIndex > -1) _Items.RemoveAt(itemIndex);
// 	}
// 	public void UpdateItem(Guid id, Item item)
// 	{
// 	var itemIndex = _Items.FindIndex(x => x.Id == id);
// 		// Proper Handeling
// 		if (itemIndex > -1) {
// 		_Items[itemIndex] = item;
// 		}
// 	}
//   }
// }
//
