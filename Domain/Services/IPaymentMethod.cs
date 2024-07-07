using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Entities;

namespace Domain.Services;

public interface IPaymentMethod
{
    Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest paymentDetails);
}
