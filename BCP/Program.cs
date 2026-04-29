using BCP.Components;
using BCP.Data;
using BCP.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Configura o serviço de injeção de dependência para BookOperation.
builder.Services.AddScoped<BookOperation>();

// Configura o serviço de injeção de dependência para IsbnLookupService,
// utilizando HttpClient para realizar as requisições à API externa.
builder.Services.AddHttpClient<IsbnLookup>();


builder.Services.AddDbContext<BookContext>(option =>
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

// Configura o caminho base da aplicação para "/bcp".
app.UsePathBase("/bcp");

// Configura o pipeline HTTP.
if (!app.Environment.IsDevelopment()) app.UseHsts();

// Aplica as migrações pendentes a base de dados.
using (IServiceScope scope = app.Services.CreateScope())
{
    DbContext db = scope.ServiceProvider.GetRequiredService<BookContext>();
    db.Database.Migrate();
}

app.UseRouting();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
