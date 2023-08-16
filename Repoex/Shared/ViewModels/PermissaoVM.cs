using System.ComponentModel.DataAnnotations;

namespace Repoex.Shared.ViewModels
{
    public class PermissaoVM
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome do Relatório é obrigatório.")]
        public string Relatorio { get; set; } = string.Empty;
    }
}
