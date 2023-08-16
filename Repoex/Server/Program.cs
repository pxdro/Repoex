global using Microsoft.EntityFrameworkCore;
global using Repoex.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repoex.Server.Context;
using Repoex.Server.Repositories.PermissaoRepository;
using Repoex.Server.Repositories.UsuarioRepository;
using Repoex.Server.Services.AcessoServices;
using Repoex.Server.Services.CotacaoServices;
using Repoex.Server.Services.DecolideiaServices;
using Repoex.Server.Services.ExportacaoServices;
using Repoex.Server.Services.HoraExtraServices;
using Repoex.Server.Services.PedidoEngenhariaServices;
using Repoex.Server.Services.TransporteServices;
using Repoex.Server.Services.UsuarioServices;
using Repoex.Server.Services.ViagensCentroTreinamentoServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//--- Meu
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDbContext<RepoexContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("RepoexConnectionStringHML"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("RepoexConnectionStringPRD"));
});

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IPermissaoRepository, PermissaoRepository>();

builder.Services.AddScoped<ICotacaoService, CotacaoService>();
builder.Services.AddScoped<IPedidoEngenhariaService, PedidoEngenhariaService>();
builder.Services.AddScoped<IAcessoService, AcessoService>();
builder.Services.AddScoped<ITransporteService, TransporteService>();
builder.Services.AddScoped<IViagensCentroTreinamentoService, ViagensCentroTreinamentoService>();
builder.Services.AddScoped<IDecolideiaService, DecolideiaService>();
builder.Services.AddScoped<IExportacaoService, ExportacaoService>();
builder.Services.AddScoped<IHoraExtraService, HoraExtraService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

//--- MEU
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
//---

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
