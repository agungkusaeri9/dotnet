using System.ComponentModel.DataAnnotations;

namespace crudmysql.DTOs.Product
{
    public class UpdateProductDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? Image { get; set; }
    }
}
