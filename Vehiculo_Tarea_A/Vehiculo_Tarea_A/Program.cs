using Vehiculo_Tarea_A.Models;
using Vehiculo_Tarea_A.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Vehiculo_Tarea_A.Services
{
    // Autor: Steveen Carabalí
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddHostedService<RabbitMQConsumer>();

            builder.Services.AddDbContext<VehiculoDBContext>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString(
                           "VehiculoConnection"
                   ))
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
