using AuthService.Data;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------------
// 1️⃣ Configuración de CORS
// ---------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlify", policy =>
    {
        policy.WithOrigins("https://eclectic-starburst-30e0ac.netlify.app") // dominio frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // para JWT o cookies
    });
});

// ---------------------
// 2️⃣ Configuración de JWT
// ---------------------
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

// ---------------------
// 3️⃣ Configuración de base de datos y servicios
// ---------------------
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------------
// 4️⃣ Swagger solo en desarrollo
// ---------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------------------
// 5️⃣ HTTPS
// ---------------------
app.UseHttpsRedirection();

// ---------------------
// 6️⃣ CORS debe ir antes de Authentication y Authorization
// ---------------------
app.UseCors("AllowNetlify");

// ---------------------
// 7️⃣ Middleware para OPTIONS preflight
// ---------------------
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

// ---------------------
// 8️⃣ Authentication y Authorization
// ---------------------
app.UseAuthentication();
app.UseAuthorization();

// ---------------------
// 9️⃣ Mapear controladores
// ---------------------
app.MapControllers();

app.Run();