using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Entities;
using Domain.Services;

namespace Application.Services.PaymentMethods;

public class CreditPayment : IPaymentMethod
{
    public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest paymentDetails)
    {
        throw new NotImplementedException();
    }
}
