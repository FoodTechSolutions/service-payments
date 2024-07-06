using Domain.Boundaries.Payments.ProcessPayment;
using Domain.entities;

namespace Domain.Services;

public interface IPaymentService
{
    Task<ProcessPaymentResponse> Process(ProcessPaymentRequest data);
}
