namespace Repoex.Shared.Models
{
    public class Cotacao
    {
        public string Fluxo { get; set; } = string.Empty;
        public string Etapa { get; set; } = string.Empty;
        public string Ciclo { get; set; } = string.Empty;
        public string Necessidade { get; set; } = string.Empty;
        public string AbertoEm { get; set; } = string.Empty;
        public string FinalizadoEm { get; set; } = string.Empty;
        public string Executor { get; set; } = string.Empty;
        public string Consumido { get; set; } = string.Empty;
    }
}
