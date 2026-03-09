using Serilog;
using StudentManagement.API.Middleware;
using StudentManagement.Application;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── 1. Logging con Serilog ───────────────────────────────────────────
builder.Host.UseSerilog((context, config) =>
    config
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));

// ── 2. Controllers y Swagger ─────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Student Management API",
        Version = "v1",
        Description = "API REST para gestion de estudiantes, cursos y matriculas"
    });
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// ── 3. CORS — permite peticiones desde Angular (puerto 4200) ─────────
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()));

// ── 4. Registrar las capas Application e Infrastructure ──────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ── 5. Aplicar migraciones de BD automaticamente al iniciar ──────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
                  .GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.Migrate();
        Log.Information("Migraciones aplicadas correctamente");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error al aplicar migraciones");
    }
}

// ── 6. Pipeline HTTP ─────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

Log.Information("API iniciada -> Swagger: https://localhost:7001/swagger");
app.Run();
