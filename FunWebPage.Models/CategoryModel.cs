using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunWebPage.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,300,ErrorMessage = "Display order must be between 1-300")]
        public int DisplayOrder { get; set; }
    }
}