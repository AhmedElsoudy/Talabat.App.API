using System.ComponentModel.DataAnnotations;

namespace TalabatApp.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage ="Price must be greater than zero")]
        public decimal Price { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Quantity must be at least one")]
        public int Quantity { get; set; }
    }
}