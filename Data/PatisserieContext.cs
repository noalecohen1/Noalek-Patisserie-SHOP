using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Patisserie.Data
{
    public class PatisserieContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PatisserieContext() : base("name=PatisserieContext")
        {
        }

        public System.Data.Entity.DbSet<Patisserie.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Patisserie.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Patisserie.Models.Account> Accounts { get; set; }

        public System.Data.Entity.DbSet<Patisserie.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Patisserie.Models.OrdersHistory> OrdersHistories { get; set; }

        public System.Data.Entity.DbSet<Patisserie.Models.Branch> Branches { get; set; }
    }
}
