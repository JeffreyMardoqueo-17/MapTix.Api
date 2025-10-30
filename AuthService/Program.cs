using   AuthService.DataBase;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AuthService.Mappings;
using AuthService.Repositories;
using AuthService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Registro de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperCompany));
builder.Services.AddAutoMapper(typeof(AutoMapperRole));

builder.Services.AddControllers();


builder.Services.AddControllers();
// Registrar servicios
builder.Services.AddScoped<ICompanyService, CompanyService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
// .WithOrigins("https://localhost:7116")
// .WithOrigins("https://localhost:5030")
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

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
