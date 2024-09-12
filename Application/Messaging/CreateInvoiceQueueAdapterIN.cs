using Application.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Application.Messaging;

public class CreateInvoiceQueueAdapterIN : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CreateInvoiceQueueAdapterIN> _logger;
    private string RABBIT_HOST;
    private string RABBIT_PORT;
    private string RABBIT_USERNAME;
    private string RABBIT_PASSWORD;

    public CreateInvoiceQueueAdapterIN(IServiceProvider serviceProvider,
        ILogger<CreateInvoiceQueueAdapterIN> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        RABBIT_HOST = configuration.GetSection("RabbitMqSettings")["HOST"] ?? string.Empty;
        RABBIT_PORT = configuration.GetSection("RabbitMqSettings")["PORT"] ?? string.Empty;
        RABBIT_USERNAME = configuration.GetSection("RabbitMqSettings")["USERNAME"] ?? string.Empty;
        RABBIT_PASSWORD = configuration.GetSection("RabbitMqSettings")["PASSWORD"] ?? string.Empty;
        CreateConnection();
    }

    private void CreateConnection()
    {

        try
        {
            _connection = new ConnectionFactory
            {
                AmqpUriSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                Endpoint = new AmqpTcpEndpoint(RABBIT_HOST, int.Parse(RABBIT_PORT)),
                UserName = RABBIT_USERNAME,
                Password = RABBIT_PASSWORD
            }.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: EventConstants.CREATE_INVOICE_EXCHANGE,
                type: ExchangeType.Direct);

            _channel.QueueDeclare(
                queue: EventConstants.CREATE_INVOICE_QUEUE);

            _channel.QueueBind(
                exchange: EventConstants.CREATE_INVOICE_EXCHANGE,
                queue: EventConstants.CREATE_INVOICE_QUEUE,
            routingKey: string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Houve um erro ao criar a conexão com o RabbitMQ. Erro: {ex.Message}");
        }
    }

    private void CreateConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        //_channel.ModelShutdown += async (s, e) => await Channel_Shutdown(s, e);
        consumer.Received += async (s, e) => await Consumer(s, e);
        _channel.BasicQos(0, 20, false);
        _channel.BasicConsume(EventConstants.CREATE_INVOICE_QUEUE, false, consumer);
    }

    public async Task Consumer(object sender, BasicDeliverEventArgs e)
    {
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
