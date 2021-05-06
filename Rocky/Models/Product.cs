using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string ShortDesc { get; set; }
		public string Description { get; set; }
		[Range(1, int.MaxValue)]
		public Double Price { get; set; }
		public string Image { get; set; }
		[Display(Name="Category Type")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]

		public virtual Category Category { get; set; }

		public int WorkId { get; set; }
		[ForeignKey("WorkId")]
		public virtual FirtsWork FirtsWork { get; set; }
	}
}
