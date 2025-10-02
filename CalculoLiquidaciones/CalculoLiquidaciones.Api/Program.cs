using CalculoLiquidaciones.Application.Interfaces;
using CalculoLiquidaciones.Application.Services;
using CalculoLiquidaciones.Domain.Interfaces;
using CalculoLiquidaciones.Infrastructure.Persistence.Contexts;
using CalculoLiquidaciones.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDConnection")));

// Repositorios
builder.Services.AddScoped<ILiquidacionRepository, LiquidacionRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();

// Servicios de Application
builder.Services.AddScoped<ILiquidacionService, LiquidacionService>();

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
