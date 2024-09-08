using Application.Configuration;
using Application.Factories;
using Application.Helpers;
using Application.Services.Interface;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRabbitMqService _rabbitMqService;

    public PaymentService(IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository, IRabbitMqService rabbitMqService)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _rabbitMqService = rabbitMqService;
    }

    public async Task<ProcessPaymentResponse> Process(ProcessPaymentRequest request)
    {
        var foundInvoice = _invoiceRepository.GetById(request.InvoiceId);

        if (foundInvoice == null)
            throw new InvalidOperationException("Invoice not found.");

        if (foundInvoice.Status.Equals(InvoiceStatus.Paid))
            throw new InvalidOperationException("Invoice is already paid.");

        IPaymentMethod paymentMethod = PaymentMethodFactory.CreatePaymentMethod(request.PaymentType);

        var payment = await paymentMethod.ProcessPaymentAsync(request);
        foundInvoice.Pay(payment.Id);

        _invoiceRepository.Update(foundInvoice);
        _paymentRepository.Add(payment);

        // Publish paid
        var rabbitRequest = new RabbitMqPublishModel<Invoice>()
        {
            ExchangeName = EventConstants.INVOICE_PAID_EXCHANGE,
            RoutingKey = string.Empty,
            Message = foundInvoice

        };
        _rabbitMqService.Publish(rabbitRequest);

        return await Task.FromResult(new ProcessPaymentResponse
        {
            PaymentId = payment.Id,
            Status = payment.Status
        });
    }
}
