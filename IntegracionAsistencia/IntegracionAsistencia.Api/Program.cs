using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Application.Services;
using IntegracionAsistencia.Domain.Interfaces;
using IntegracionAsistencia.Infrastructure;
using IntegracionAsistencia.Infrastructure.Messaging;
using IntegracionAsistencia.Infrastructure.Persistence.Contexts;
using IntegracionAsistencia.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.License.SetNonCommercialPersonal("Manuel - Prueba Técnica");

// DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDConnection")));

builder.Services.AddMessaging(builder.Configuration);
builder.Services.Configure<RabbitMqOpciones>(builder.Configuration.GetSection("RabbitMq"));

// Inyección de dependencias (repositorios y servicios)
builder.Services.AddScoped<IAsistenciaRepository, AsistenciaRepository>();
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();
builder.Services.AddScoped<IAsistenciaCargaService, AsistenciaCargaService>();
builder.Services.AddScoped<IResumenAsistenciaRepository, ResumenAsistenciaRepository>();
builder.Services.AddScoped<IResumenAsistenciaService, ResumenAsistenciaService>();


// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "IntegracionAsistencia API", Version = "v1" });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IntegracionAsistencia API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
