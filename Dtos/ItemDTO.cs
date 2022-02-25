using System;
using System.ComponentModel.DataAnnotations;
namespace API.Dtos
{
	public class ItemDTO
	{
			public Guid Id { get; set; }
			[Required]
			public string? Title { get; set; }
			[Required]
			[Range(0, 100)]
			public decimal Price { get; set; }
			[Required]
			public string? Note { get; set; }
	}
}

