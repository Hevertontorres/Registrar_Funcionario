using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Registrar_Funcionario.Models
{
    public class Context : DbContext
    {
        public Context() : base("Data Source=localhost;Initial Catalog=Registrar_Funcionario;Integrated Security=True")
        {

        }

        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Properties().Where(x => x.Name == x.ReflectedType.Name + "Id").Configure(y => y.IsKey());
            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(x => x.HasMaxLength(100));
            modelBuilder.Entity<Funcionario>().Property(x => x.Salario).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}