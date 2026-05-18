using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SoftwareVersionManager.Data;
using SoftwareVersionManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

// Logs
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Swagger/OpenAPI configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Software Version Manager API",
        Version = "v1.0.0",
        Description = "API REST para gerenciamento de versões de software",
        Contact = new OpenApiContact
        {
            Name = "Bruno Mazetto",
            Url = new Uri("https://linkedin.com/in/brunomztt")
        }
    });
});

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    );
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("SoftwareVersionManager")
    );
}

// Dependency Injection
builder.Services.AddScoped<ISoftwareService, SoftwareService>();
builder.Services.AddScoped<ISoftwareVersionService, SoftwareVersionService>();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger habilitado sempre
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Software Version Manager API v1.0.0");
    options.RoutePrefix = string.Empty;
});

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.MapHealthChecks("/health");

// Seed + Migration automática
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("puxando os dados pra facilitar nos testes :D");

        if (app.Environment.IsProduction())
        {
            context.Database.Migrate();
        }

        DbInitializer.Seed(context);

        logger.LogInformation("Banco de dados inicializado com sucesso");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao inicializar banco de dados");
    }
}

app.Run();