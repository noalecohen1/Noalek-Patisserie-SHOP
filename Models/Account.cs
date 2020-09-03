using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Patisserie.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [DisplayName("Account type")]
        public bool IsModerator { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
  
        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }


        // Many-to-Many Relation
        public IList<Order> Orders { get; set; }
        public List<OrdersHistory> OrdersHistory { get; set; }
    }
}