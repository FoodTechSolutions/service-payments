using Application.Services;
using Domain.Services;

namespace Application;

using Application.BackgroundServices;
using Application.Services.Interface;
using APPLICATION.Service;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ICreateInvoiceService, CreateInvoiceService>();


        services.AddScoped<IRabbitMqService, RabbitMqService>();

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<CreateInvoiceHandler>();

        return services;
    }
}
