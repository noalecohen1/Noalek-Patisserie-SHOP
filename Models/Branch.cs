using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Patisserie.Models
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set;  }
    }
}