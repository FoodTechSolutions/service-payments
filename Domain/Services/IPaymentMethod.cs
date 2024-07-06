using Domain.Boundaries.Payments.ProcessPayment;

namespace Domain.Services;

public interface IPaymentMethod
{
    Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest paymentDetails);
}
