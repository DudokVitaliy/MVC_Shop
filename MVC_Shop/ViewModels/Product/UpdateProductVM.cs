using System.ComponentModel.DataAnnotations;

namespace MVC_Shop.ViewModels.Product
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        [MaxLength(100, ErrorMessage = "Максимальна к-сть символів 100!")]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Ціна не може бути пустою!")]
        [Range(0, Double.MaxValue, ErrorMessage = "Значення не може бути меншим за 0!")]
        public double? Price { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "К-сть не може бути пустою!")]
        [Range(0, int.MaxValue, ErrorMessage = "Значення не може бути меншим за 0!")]
        public int? Count { get; set; }
        public int CategoryId { get; set; }
        public string? UniqueNameError { get; set; }
    }
}
