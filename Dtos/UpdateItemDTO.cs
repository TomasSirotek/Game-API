using System;
using System.ComponentModel.DataAnnotations;
namespace API.Dtos
{
	public class UpdateItemDTO
	{
			[Required]
			public string? Title { get; set; }
			[Required]
			[Range(0, 100)]
			public decimal Price { get; set; }
			public string?  Note { get; set; }
	}
}

