using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {

        //recorto el configure asi
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            //aqui implemento el nombre de tabla como context y annado los parametros
            builder.ToTable("Producto");

            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
                //Una clave Foranea
            builder.HasOne(p => p.Marca)
                .WithMany(p => p.Productos)//hayque hacerlo para que vea la foreign key
                .HasForeignKey(p => p.MarcaId);

            builder.HasOne(p => p.Categoria)
                .WithMany(p => p.Productos)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
