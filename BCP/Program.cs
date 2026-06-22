using BCP.Components;
using BCP.Data;
using BCP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Configura o serviço de injeção de dependência para BookOperation.
builder.Services.AddScoped<BookOperation>();

// Configura o serviço de injeção de dependência para IsbnLookupService,
// utilizando HttpClient para realizar as requisições à API externa.
builder.Services.AddHttpClient<IsbnLookup>();

builder.Services.AddDbContext<BookContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configura o pipeline HTTP.
if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseRouting();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();