using Application.Configuration;
using Application.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services;

public class RabbitMqService(ILogger<RabbitMqService> logger, IConfiguration configuration) : IRabbitMqService
{
    private IConnection _connection;
    private IModel _channel;
    private string RABBIT_HOST = configuration.GetSection("RabbitMqSettings")["HOST"] ?? string.Empty;
    private string RABBIT_PORT = configuration.GetSection("RabbitMqSettings")["PORT"] ?? string.Empty;
    private string RABBIT_USERNAME = configuration.GetSection("RabbitMqSettings")["USERNAME"] ?? string.Empty;
    private string RABBIT_PASSWORD = configuration.GetSection("RabbitMqSettings")["PASSWORD"] ?? string.Empty;

    private void CreateConnection()
    {
        try
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            if (_channel is not null) return;

            _connection = new ConnectionFactory
            {
                AmqpUriSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                Endpoint = new AmqpTcpEndpoint(RABBIT_HOST, int.Parse(RABBIT_PORT)),
                UserName = RABBIT_USERNAME,
                Password = RABBIT_PASSWORD
            }.CreateConnection();

            _channel = _connection.CreateModel();
        }
        catch (Exception ex)
        {
            logger.LogError("Error while creating RabbitMq Connection.");
            logger.LogError(ex.Message, ex);
        }
    }

    public void Publish<T>(RabbitMqPublishModel<T> rabbitMqConfig)
    {
        try
        {
            CreateConnection();
            var body = EncodingMessage(rabbitMqConfig.Message);
            _channel.BasicPublish(
                rabbitMqConfig.ExchangeName,
                rabbitMqConfig.RoutingKey,
                false,
                null,
                body
            );
        }
        catch (Exception ex)
        {
            logger.LogError("Error while publish RabbitMq.");
            logger.LogError(ex.Message, ex);
        }
    }

    private byte[] EncodingMessage<T>(T message)
    {
        var jsonSerialize = JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(jsonSerialize);
    }
}
