using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class CreateRestaurantDto
    {
        [Required]                                      // <-- Walidacja wprowadzonych danych
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool HasDelivery { get; set; }

        public string ContactEmail { get; set; }

        public string ContactNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        public string PostalCode { get; set; }
    }
}
