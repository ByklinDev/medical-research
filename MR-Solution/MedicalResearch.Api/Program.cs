using System.Reflection;
using FluentValidation;
using MedicalResearch.DAL.DataContext;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<User>, UserValidator>();


DALRegistrator.RegisterService(builder.Services, builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
