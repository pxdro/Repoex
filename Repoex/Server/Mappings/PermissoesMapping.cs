using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repoex.Server.Mappings
{
    public class PermissoesMapping : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasMany(builder => builder.Usuarios)
                .WithMany(builder => builder.Permissoes);

            builder.ToTable("Permissoes");
        }
    }
}
