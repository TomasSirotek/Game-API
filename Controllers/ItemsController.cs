﻿using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;
using API.Models;
using API.Repositories;

// Service controller => ALL HTTP REQUESTS

namespace API.Controllers
{
	[ApiController]
	[Route("[controller]")] 

	public class ItemsController : ControllerBase
	{
		// Using interface repo
		private IItems _ItemRepo;

		// Constructor with dependen inject
		public ItemsController(IItems itemRepo)
		{
			// For loading only once 
			_ItemRepo = itemRepo;
			//_ItemRepo = new InMemItemRepo();
		}

		// Get All items
		[HttpGet]
		public ActionResult<IEnumerable<ItemDTO>> GetItems()
		{
			return _ItemRepo.GetItems()
				.Select(x => new ItemDTO { Id = x.Id, Title = x.Title, Price = x.Price , Note = x.Note })
				.ToList();
		}

		[HttpGet("{id}")]
		public ActionResult<ItemDTO> GetItemById(Guid id)// structs 
		{
			var item = _ItemRepo.GetItemById(id);

			if (item == null) return NotFound();

			var itemDTO = new ItemDTO {
				Id = item.Id,
				Title = item.Title,
				Price = item.Price,
				Note = item.Note
			};

			return itemDTO;
		}

		[HttpPost]
		public ActionResult CreateItem(CreateItemDTO item)
		{
			var existingItem = new Item();
			existingItem.Id = Guid.NewGuid();
			existingItem.Title = item.Title;
			existingItem.Price = item.Price;
			existingItem.Note = item.Note;

			// Function for creating a book
			_ItemRepo.CreateItem(existingItem);

			return Ok();
		}

		[HttpPut("{id}")]
		public ActionResult UpdateItem(Guid id, UpdateItemDTO item)
		{
			// fetching single game from the repo 
			var existingItem = _ItemRepo.GetItemById (id);

			if (existingItem == null) return NotFound();

			existingItem.Title = item.Title;
			existingItem.Price = item.Price;
			existingItem.Note = item.Note;

			// Function for creating a book
			_ItemRepo.UpdateItem(id, existingItem);

			return NoContent();
		}

		// [HttpDelete("{id}")]
		// public ActionResult DeleteItem(Guid id)
        // {
		// 	var existingItem = _ItemRepo.GetItemById(id);

		// 	if (existingItem is null) {
		// 		return NotFound();
		// 	}

		// 	_ItemRepo.DeleteItem(id);

		// 	return NoContent();
		// }
	}
}