using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConversorMoedas.Data
{
    public class ConversorMoedasContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ConversorMoedasContext() : base("name=ConversorMoedasContext")
        {
        }

        public System.Data.Entity.DbSet<ConversorMoedas.Models.Transaction> Transactions { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConversorMoedas.Models.Transaction>().Property(x => x.OriginValue).HasPrecision(18, 4); //Format precision number 
            modelBuilder.Entity<ConversorMoedas.Models.Transaction>().Property(x => x.ConversionRate).HasPrecision(18, 4); //Format precision number
        }
    }
}
