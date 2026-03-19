using BCP.Components;
using BCP.Data;
using BCP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.
builder.Services.AddRazorComponents().
    AddInteractiveServerComponents();

builder.Services.AddDbContext<BookContext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("ConexaoBD") ??
        throw new InvalidOperationException("A string de conexão não foi configurada corretamente.")));

// Configura o serviço de injeção de dependência para BookOperation.
builder.Services.AddScoped<BookOperation>();

var app = builder.Build();

// Configura o pipeline de solicitações HTTP.
if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().
    AddInteractiveServerRenderMode();
app.Run();
