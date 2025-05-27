using System.ComponentModel.DataAnnotations;

namespace technicalTes_Nawatech.Models
{
	public class ProductCategory
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
	}
}
