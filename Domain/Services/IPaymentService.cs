using Domain.Boundaries.Payments.ProcessPayment;

namespace Domain.Services;

public interface IPaymentService
{
    Task<ProcessPaymentResponse> Process(ProcessPaymentRequest data);
}
