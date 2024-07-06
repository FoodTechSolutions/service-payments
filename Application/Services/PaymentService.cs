using Application.Services.PaymentMethods;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Services;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<ProcessPaymentResponse> Process(ProcessPaymentRequest request)
        {
            IPaymentMethod paymentMethod = PaymentMethodFactory.CreatePaymentMethod(request.PaymentType);

            var payment = await paymentMethod.ProcessPaymentAsync(request);

            return await Task.FromResult(new ProcessPaymentResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status
            });
        }
    }
}

public static class PaymentMethodFactory
{
    public static IPaymentMethod CreatePaymentMethod(PaymentType paymentType)
    {
        switch (paymentType)
        {
            case PaymentType.Debit:
                return new DebitPayment();
            case PaymentType.Credit:
                return new CreditPayment();
            case PaymentType.Pix:
                return new PixPayment();
            default:
                throw new NotSupportedException($"Payment type '{paymentType}' is not supported.");
        }
    }
}
