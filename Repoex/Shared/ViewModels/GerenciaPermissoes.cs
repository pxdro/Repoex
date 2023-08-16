namespace Repoex.Shared.ViewModels
{
    public class GerenciaPermissoes
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Relatorio { get; set; } = string.Empty;
        public bool Include { get; set; } = false;
    }
}
