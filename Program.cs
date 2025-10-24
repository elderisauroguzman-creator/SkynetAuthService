using AuthService.Data;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularDev", policy =>
//    {
//        policy.WithOrigins("http://localhost:4200",
//            "http://localhost:5023",
//            "http://localhost:53537",
//            "http://localhost:63796",
//             "http://localhost:57016") // <-- la URL de tu Angular
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlifyOrigin", policy =>
    {
        policy.WithOrigins("https://eclectic-starburst-30e0ac.netlify.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // si usas cookies o autenticación
    });
});

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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

// DB Context
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<AuthService.Interfaces.IAuthService, AuthService.Services.AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowNetlifyOrigin");

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");
        options.RoutePrefix = string.Empty; // opcional: para que abra en "/"
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();