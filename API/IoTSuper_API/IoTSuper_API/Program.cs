using IoTSuper_API.Data;
using IoTSuper_API.DTO;
using IoTSuper_API.Security;
using IoTSuper_API.Services;
using IoTSuper_API.Services.Interface;
using IoTSuper_API.Services.Worker;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using AutenticacionBasica = IoTSuper_API.Security.AutenticacionBasica;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("MariaDb");

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.Configure<AutenticacionBasica>(builder.Configuration.GetSection(AutenticacionBasica.SectionName));

builder.Services.AddSingleton<Crypto>(sp =>
{
    IConfiguration config = sp.GetRequiredService<IConfiguration>();
    IConfiguration section = config.GetSection(Crypto.SectionName);
    string clave = section.GetValue<string>("claveEncriptacion") ?? string.Empty;
    string vector = section.GetValue<string>("vectorEncriptacion") ?? string.Empty;
    return new Crypto(vector, clave);
});

builder.Services.Configure<ConfiguracionEmail>(
    builder.Configuration.GetSection("ConfiguracionEmail")
);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/var/api/keys"))
    .SetApplicationName("IoTSuperAPI");

builder.Services.AddHostedService<EventoWorker>();

builder.Services.AddScoped<IContrasenaService, ContrasenaService>();
builder.Services.AddScoped<ICentroService, CentroService>();
builder.Services.AddScoped<ISeccionService, SeccionService>();
builder.Services.AddScoped<IComponenteService, ComponenteService>();

builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, AutentificacionBasicaHandler>("BasicAuth", null);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    db.Database.Migrate(); // Aplica migraciones pendientes al iniciar
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();