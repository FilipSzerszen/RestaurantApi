using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models
{
    public class ModifyRestaurantDto
    {
        [Required]                                      // <-- Walidacja wprowadzonych danych
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool HasDelivery { get; set; }

    }
}
