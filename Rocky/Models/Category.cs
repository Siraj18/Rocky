using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Rocky.Models
{
	public class Category
	{
		public int CategoryId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Display Order must be grater than 0")]
		public int DisplayOrder { get; set; }


	}
}
