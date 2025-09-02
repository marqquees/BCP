using BibliotecaCarvalhoPereira.Components;
using BibliotecaCarvalhoPereira.Data;
using BibliotecaCarvalhoPereira.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();

builder.Services.AddDbContext<LivroContext>(options => 
options.UseMySql(
    builder.Configuration.GetConnectionString("ConexaoBD") ?? 
        throw new InvalidOperationException("A string de conexão não foi configurada corretamente."),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ConexaoBD"))
    ));

// Configura o serviço de injeção de dependência para LivroRepository.
builder.Services.AddScoped<LivroService>();

var app = builder.Build();

// Configura o pipeline de solicitações HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Erro", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
