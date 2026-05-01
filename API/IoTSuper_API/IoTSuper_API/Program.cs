using IoTSuper_API.Data;
using IoTSuper_API.Security;
using IoTSuper_API.Services;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authentication;
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

builder.Services.AddScoped<IContrasenaService, ContrasenaService>();

builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, AutentificacionBasicaHandler>("BasicAuth", null);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();