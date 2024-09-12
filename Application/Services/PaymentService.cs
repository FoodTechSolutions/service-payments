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
        //var foundInvoice = _invoiceRepository.GetById(request.InvoiceId);

        //if (foundInvoice == null)
        //    throw new InvalidOperationException("Invoice not found.");

        //if (foundInvoice.Status.Equals(InvoiceStatus.Paid))
        //    throw new InvalidOperationException("Invoice is already paid.");

        //IPaymentMethod paymentMethod = PaymentMethodFactory.CreatePaymentMethod(request.PaymentType);

        //var payment = await paymentMethod.ProcessPaymentAsync(request);
        //foundInvoice.Pay(payment.Id);

        //_invoiceRepository.Update(foundInvoice);
        //_paymentRepository.Add(payment);

        // Publish paid
        var rabbitRequest = new RabbitMqPublishModel<CreateProduction>()
        {
            ExchangeName = EventConstants.PAID_EXCHANGE,
            RoutingKey = string.Empty,
            Message = new CreateProduction
            {
                OrderId = request.InvoiceId,
            }

        };
        _rabbitMqService.Publish(rabbitRequest);

        return await Task.FromResult(new ProcessPaymentResponse
        {
            PaymentId = Guid.Parse("dae5b898-209d-42f6-a93e-088c35b3c528"),
            Status = PaymentStatus.Completed
        });
    }

    public class CreateProduction()
    {
        public Guid OrderId { get; set; }
    }
}
