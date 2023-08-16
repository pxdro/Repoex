namespace Repoex.Server.Context
{
    public class RepoexContext : DbContext
    {
        public RepoexContext(DbContextOptions<RepoexContext> options) : base(options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepoexContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
