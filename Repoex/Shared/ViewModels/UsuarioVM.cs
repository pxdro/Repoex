using Repoex.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Repoex.Shared.ViewModels
{
    public class UsuarioVM
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome do Usuário é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Login do Usuário é obrigatório.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "É obrigatório informar se o usuário é Admin.")]
        public EnumStatus Admin { get; set; }

        [Required(ErrorMessage = "Status do Usuário é obrigatório.")]
        public EnumStatus Status { get; set; }

        /* Relações com as Permissões - EF Core */
        public IEnumerable<PermissaoVM>? Permissoes { get; set; }
    }
}
