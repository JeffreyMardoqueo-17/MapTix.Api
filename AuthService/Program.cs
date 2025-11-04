using AuthService.DataBase;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AuthService.Mappings;
using AuthService.Repositories;
using AuthService.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using AuthService.helpers; // 👈 Necesario para Encoding.UTF8

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Corregido: usa builder.Configuration en lugar de 'configuration'
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Jwt:Issuer"],
           ValidAudience = builder.Configuration["Jwt:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
           )
           ///FALTA ESTO EN APPSETTINGS.JSON:
           /// {
           ///   "Jwt": {
           ///     "Key": "TuClaveSecretaDe32CaracteresOMas",
           ///     "Issuer": "tuapp.com",
           ///     "Audience": "tuapp.com"
           ///   }
           /// }
       };
   });

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtHelper>(); // Agregar JwtHelper como servicio
 

//Registro de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperCompany));
builder.Services.AddAutoMapper(typeof(AutoMapperRole));

builder.Services.AddControllers();


builder.Services.AddControllers();
// Registrar servicios
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IRoleService, RoleService>();

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
