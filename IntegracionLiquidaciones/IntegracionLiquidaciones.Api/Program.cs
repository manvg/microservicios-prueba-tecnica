using IntegracionLiquidaciones.Application.Interfaces;
using IntegracionLiquidaciones.Application.Services;
using IntegracionLiquidaciones.Domain.Interfaces;
using IntegracionLiquidaciones.Infrastructure.Persistence.Contexts;
using IntegracionLiquidaciones.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDConnection")));

// Inyección de dependencias (repositorios y servicios)
builder.Services.AddScoped<IResumenAsistenciaRepository, ResumenAsistenciaRepository>();
builder.Services.AddScoped<IResumenAsistenciaService, ResumenAsistenciaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
