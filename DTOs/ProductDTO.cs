using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        [Required, MaxLength(30, ErrorMessage = "name must be less than 30 characters long")]
        public string ProductName { get; set; } = null!;
        [Required]
        public float Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}
