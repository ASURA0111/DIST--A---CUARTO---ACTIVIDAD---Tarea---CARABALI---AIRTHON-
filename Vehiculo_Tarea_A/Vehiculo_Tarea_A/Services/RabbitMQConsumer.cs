using Vehiculo_Tarea_A.Models;
using Vehiculo_Tarea_A.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Vehiculo_Tarea_A.Events;

namespace Vehiculo_Tarea_A.Services
{
    // Autor: Steveen Carabalí
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQConsumer(
            IConfiguration configuration,
            ILogger<RabbitMQConsumer> logger,
            IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            var queueName = _configuration["RabbitMQ:QueueName"]!;

            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensaje = Encoding.UTF8.GetString(body);

                var evento = JsonSerializer.Deserialize<CategoriaCreadaEvento>(mensaje);

                if (evento != null)
                {
                    _logger.LogInformation(
                        "Categoría creada recibida. IdCategoria: {IdCategoria}",
                        evento.IdCategoria
                    );

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<VehiculoDBContext>();

                    // Obtener la lista y verificar existencia mediante bucle tradicional
                    var listaVehiculos = await dbContext.vehiculos.ToListAsync();
                    bool existe = false;

                    foreach (var item in listaVehiculos)
                    {
                        if (item.IdCategoria == evento.IdCategoria)
                        {
                            existe = true;
                            break;
                        }
                    }

                    if (!existe)
                    {
                        var vehiculo = new Vehiculo
                        {
                            IdCategoria = evento.IdCategoria,
                            Marca = "Sin asignar",
                            Modelo = "Sin asignar",
                            Precio = 0m, // La 'm' indica que es de tipo decimal
                            Stock = 0,
                            Estado = true
                        };

                        dbContext.vehiculos.Add(vehiculo);
                        await dbContext.SaveChangesAsync();

                        _logger.LogInformation(
                            "Vehículo base creado automáticamente para IdCategoria: {IdCategoria}",
                            evento.IdCategoria
                        );
                    }
                }

                await _channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
                );
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}