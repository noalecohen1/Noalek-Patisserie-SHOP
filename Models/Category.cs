using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Patisserie.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Category image")]
        public string ImagePath { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        //One-to-Many Relation
        public IList<Product> Products { get; set; }

    }
}