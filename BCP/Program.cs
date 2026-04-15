using BCP.Components;
using BCP.Data;
using BCP.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddDbContext<BookContext>(option =>
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura o serviço de injeção de dependência para BookOperation.
builder.Services.AddScoped<BookOperation>();

// Configura o serviço de injeção de dependência para IsbnLookupService,
// utilizando HttpClient para realizar as requisições à API externa.
builder.Services.AddHttpClient<IsbnLookup>();

WebApplication app = builder.Build();

// Aplica as migrações pendentes a base de dados.
using (IServiceScope scope = app.Services.CreateScope())
{
    DbContext db = scope.ServiceProvider.GetRequiredService<BookContext>();
    db.Database.Migrate();
}

// Configura o pipeline HTTP.
if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
