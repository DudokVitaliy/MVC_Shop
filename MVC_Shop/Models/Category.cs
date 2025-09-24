using System.ComponentModel.DataAnnotations;

namespace MVC_Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        // requared - обов`язкове поле
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        public List<Product> Products { get; set; } = [];
    }
}
