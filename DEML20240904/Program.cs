using DEML20240904.Auth;
using DEML20240904.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Swagger para documentar la API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    // Configurar un esquema de seguridad para JWT en Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar tu token de JWT Authentication",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// Configurar políticas de autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoggedInPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

// Definir la clave secreta utilizada para firmar y verificar tokens JWT
var key = "Key.JWTAPIMinimal2023.API";

// Configurar la autenticación con JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false
    };
});

// Agregar una instancia única del servicio de autenticación JWT
builder.Services.AddSingleton<IJwtAuthenticationService>(new JwtAuthenticationService(key));

var app = builder.Build();

// Configurar Swagger en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Agregar los endpoints de la aplicación
app.AddAccountEndpoints();
app.AddCategoriaProductoEndpoints();
app.AddBodegaEndpoints();

// Configurar redirección HTTPS
app.UseHttpsRedirection();

// Habilitar la autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Ejecutar la aplicación
app.Run();