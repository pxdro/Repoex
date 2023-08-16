global using Microsoft.AspNetCore.Components.Authorization;
global using Repoex.Shared.Models;
global using Repoex.Shared.ViewModels;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Repoex.Client;
using Repoex.Client.Authentication;
using Repoex.Client.Services.AcessoServices;
using Repoex.Client.Services.CotacaoServices;
using Repoex.Client.Services.DecolideiaServices;
using Repoex.Client.Services.ExportacaoServices;
using Repoex.Client.Services.HoraExtraServices;
using Repoex.Client.Services.PedidoEngenhariaServices;
using Repoex.Client.Services.PermissaoServices;
using Repoex.Client.Services.TransporteServices;
using Repoex.Client.Services.TrasporteServices;
using Repoex.Client.Services.UsuarioServices;
using Repoex.Client.Services.ViagensCentroTreinamentoServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPermissaoService, PermissaoService>();
builder.Services.AddScoped<ICotacaoService, CotacaoService>();
builder.Services.AddScoped<IPedidoEngenhariaService, PedidoEngenhariaService>();
builder.Services.AddScoped<IAcessoService, AcessoService>();
builder.Services.AddScoped<ITransporteService, TransporteServices>();
builder.Services.AddScoped<IViagensCentroTreinamentoService, ViagensCentroTreinamentoServices>();
builder.Services.AddScoped<IDecolideiaService, DecolideiaService>();
builder.Services.AddScoped<IExportacaoService, ExportacaoService>();
builder.Services.AddScoped<IHoraExtraService, HoraExtraService>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();