using System;
using System.ComponentModel.DataAnnotations;
namespace API.Models
{
	public class Item
	{
		public Guid Id { get; set; }
		public string? Title { get; set; }
		[Required]
		[Range(0, 100)]
		public decimal Price { get; set; }
		[Required]
		public string? Note { get; set; }
	}
}
