using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Up_Img.Models.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Img_Path { get; set; }
        [DisplayName("Brand")]
        public Guid BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? BrandNames { get; set; }

    }
}
