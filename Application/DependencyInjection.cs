using Application.Services;
using Domain.Services;

namespace Application;

using Application.BackgroundServices;
using Application.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IProcessEventExampleService, ProcessEventExampleService>();


        services.AddSingleton<IRabbitMqService, RabbitMqService>();

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMqExampleHandler>();
        return services;
    }
}
