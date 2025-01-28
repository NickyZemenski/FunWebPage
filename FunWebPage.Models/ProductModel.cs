using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunWebPage.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [DisplayName("Book Title")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN {  get; set; }
        [Required]
        public string Author {  get; set; }
        [Required]
        [DisplayName("List Price")]
        public double ListPrice {  get; set; }

        [Required]
        [DisplayName("Price for 1-50")]
        public double Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        public double Price50 { get; set; }
        [Required]
        [DisplayName("Price for 100+")]
        public double Price100 { get; set; }




    }
}
