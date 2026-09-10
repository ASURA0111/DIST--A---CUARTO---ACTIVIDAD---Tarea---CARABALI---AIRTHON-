using Categoria_Tarea_A.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Categoria_Tarea_A
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // (RabbitMQ eliminado de aquí)

            builder.Services.AddDbContext<CategoriaDBContext>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString(
                           "CategoriaConnection"
                   ))
            );

            builder.Services.AddEndpointsApiExplorer();

            // --- CONFIGURACIÓN DE SWAGGER CON CANDADO (AUTHORIZE) ---
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Categoria_Tarea_A", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingresa 'Bearer' seguido de un espacio y tu token JWT.\r\n\r\nEjemplo: Bearer eyJhbGciOiJIUzI1...",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configuración de Autenticación JWT
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            var app = builder.Build();

            // Aplicar migraciones automáticamente a la base de datos de Azure al arrancar
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CategoriaDBContext>();
                dbContext.Database.Migrate();
            }

            // Swagger habilitado siempre para evitar errores 404 en Azure
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            // Activar Autenticación antes de la Autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}