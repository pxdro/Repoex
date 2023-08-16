using Microsoft.IdentityModel.Tokens;
using Repoex.Server.Repositories.UsuarioRepository;
using Repoex.Shared.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Repoex.Server.Services.UsuarioServices
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        // INJECAO DA DLL DO ACTIVE DIRECTORY
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        private static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);
        // ---------------------------------

        public async Task<Usuario?> Logar(UsuarioDto usuario)
        {
            var user = (await _usuarioRepository.Buscar(user => user.Login == usuario.Login)).FirstOrDefault();
            bool isValid = false;

            if (user != null && user.Status == EnumStatus.Ativo)
            {
                IntPtr tokenHandler = IntPtr.Zero;
                isValid = LogonUser(usuario.Login, "--sensitive-data", usuario.Senha, 2, 0, ref tokenHandler);

                if (isValid == true)
                {
                    user = await _usuarioRepository.ObterUsuarioPorIdPermissoes(user.Id);
                    return user;
                }
            }

            return null;
        }

        public async Task<Usuario?> Adicionar(Usuario usuario)
        {
            var usuarios = (await _usuarioRepository.ObterTodos());

            foreach (var usu in usuarios)
            {
                if (usu.Login == usuario.Login)
                    return null;
            }

            await _usuarioRepository.Adicionar(usuario);

            return usuario;
        }

        public async Task<Usuario?> Atualizar(Usuario usuario)
        {
            var usuarios = (await _usuarioRepository.ObterTodos());

            foreach (var usu in usuarios)
            {
                if (usu.Login == usuario.Login && usu.Id != usuario.Id)
                    return null;
            }

            await _usuarioRepository.Atualizar(usuario);

            return usuario;
        }
        public string CreateToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Login),
                new Claim(ClaimTypes.Role, (usuario.Admin == EnumStatus.Ativo ? "Admin" : "Basico"))
            };

            if (usuario.Permissoes != null)
            {
                foreach (var permissao in usuario.Permissoes)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permissao.Relatorio));
                }
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}
