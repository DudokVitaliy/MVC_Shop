using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace MVC_Shop.ViewModels.Category
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "Вкажіть ім`я!")]
        [MaxLength(50, ErrorMessage = "Максимальна кількість символів 50!")]
        public required string Name { get; set; }
        public string? UniqueNameError { get; set; }    
    }
}
