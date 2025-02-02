using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunWebPage.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public String Name { get; set; }
        public String? StreetAddress { get; set; }
        public String? City { get; set; }
        public String? State { get; set; }
        public String? PostalCode { get; set; }
        public String? PhoneNumber { get; set; }
    }
}

