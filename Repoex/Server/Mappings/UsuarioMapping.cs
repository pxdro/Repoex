using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repoex.Server.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Nome)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(c => c.Login)
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder.Property(c => c.Admin)
                .HasConversion<string>()
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder
                .HasMany(builder => builder.Permissoes)
                .WithMany(builder => builder.Usuarios);

            builder.ToTable("Usuarios");
        }
    }
}
