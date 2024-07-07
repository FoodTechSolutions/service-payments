using Application.Factories;
using Application.Services.PaymentMethods;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public PaymentService(IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
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

        return await Task.FromResult(new ProcessPaymentResponse
        {
            PaymentId = payment.Id,
            Status = payment.Status
        });
    }
}
