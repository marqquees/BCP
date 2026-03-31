using BCP.Components;
using BCP.Data;
using BCP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddDbContext<BookContext>(option =>
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura o serviço de injeção de dependência para BookOperation.
builder.Services.AddScoped<BookOperation>();

var app = builder.Build();

// Aplica as migrações pendentes a base de dados.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookContext>();
    db.Database.Migrate();
}

// Configura o pipeline HTTP.
if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
