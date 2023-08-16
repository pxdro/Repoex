using AutoMapper;
using Repoex.Shared.ViewModels;

namespace Repoex.Server.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioVM>().ReverseMap();
            CreateMap<Permissao, PermissaoVM>().ReverseMap();
        }
    }
}
