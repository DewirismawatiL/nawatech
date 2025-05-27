using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace technicalTes_Nawatech.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Image { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        public ProductCategory Category { get; set; }



        public bool IsDeleted { get; set; } = false;
    }
}
