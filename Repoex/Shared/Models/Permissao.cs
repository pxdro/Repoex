using System.ComponentModel.DataAnnotations;

namespace Repoex.Shared.Models
{
    public class Permissao : Entity
    {
        [Required(ErrorMessage = "Nome do Relatório é obrigatório.")]
        public string Relatorio { get; set; } = string.Empty;

        /* Relações com os Usuários - EF Core */
        public IEnumerable<Usuario>? Usuarios { get; set; }
    }
}
