using System.Reflection;
using FluentValidation;
using MedicalResearch.Api;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.Middleware;
using MedicalResearch.DAL.DataContext;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Services;
using MedicalResearch.Domain.Validations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "Medical Research Api",
    Title = "Medical Research",
    Version = "v1",
}));

builder.Services.AddAutoMapper(typeof(AppAutoMapperProfile).Assembly);
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Clinic>, ClinicValidator>();
builder.Services.AddScoped<IValidator<DosageForm>, DosageFormValidator>();
builder.Services.AddScoped<IValidator<MedicineContainer>, MedicineContainerValidator>();
builder.Services.AddScoped<IValidator<MedicineType>, MedicineTypeValidator>();
builder.Services.AddScoped<IValidator<Medicine>, MedicineValidator>();
builder.Services.AddScoped<IValidator<Patient>, PatientValidator>();
builder.Services.AddScoped<IValidator<Role>, RoleValidator>();
builder.Services.AddScoped<IValidator<UserCreateDTO>, CreateUserDTOValidator>();
builder.Services.AddScoped<IValidator<Supply>, SupplyValidator>();

builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IDosageFormService, DosageFormService>();
builder.Services.AddScoped<IMedicineContainerService, MedicineContainerService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IMedicineTypeService, MedicineTypeService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IVisitService, VisitService>();


DALRegistrator.RegisterService(builder.Services, builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
