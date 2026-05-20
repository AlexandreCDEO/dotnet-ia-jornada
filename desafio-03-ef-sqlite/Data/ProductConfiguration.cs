using desafio_03_ef_sqlite.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace desafio_03_ef_sqlite.Data;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
       builder.ToTable("Products");
       
       builder.HasKey(p => p.Id);
       
       builder.Property(x => x.Name)
           .HasMaxLength(100)
           .IsRequired();

       builder.HasIndex(x => x.Name)
           .IsUnique();
       
       builder.Property(x => x.Price)
           .HasPrecision(18, 2)
           .IsRequired();
    }
}