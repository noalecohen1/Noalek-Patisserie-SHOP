using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Patisserie.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Image")]
        public string ImagePath { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        [DisplayName("Dairy")]
        public bool IsDairy { get; set; }

        [DisplayName("Gluten Free")]
        public bool IsGlutenFree { get; set; }

        [DisplayName("Vegan")]
        public bool IsVegan { get; set; }

        [DisplayName("Popularity Rate")]
        public int PopularityRate { get; set; }

        //One-to-Many Relation
        [DisplayName("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public IList<Order> Orders { get; set; }

        public IList<OrdersHistory> OrdersHistory { get; set; }
    }
}