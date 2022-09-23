using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }


        public DbSet<Produto> Produto { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FAZ COM QUE TODOS OS CAMPOS DO TIPO STRING QUE NÃO TENHAM O MAPPING CONFIGURADO ASSUMAM ESTA CONFIGURAÇÃO
            foreach(var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                s => s.GetProperties().Where(tipoColuna => tipoColuna.ClrType == typeof(string))
                ))
            {
                property.SetColumnType("varchar(10)");
            }

            //FAZ COM QUE ELE BUSQUE AS CONFIGURAÇÕES DAS TABELAS QUE VÃO SER CRIADAS ATRAVÉS DE TODAS AS CLASSES QUE HERDEM DE IEntityTypeConfiguration<type>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }
        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
