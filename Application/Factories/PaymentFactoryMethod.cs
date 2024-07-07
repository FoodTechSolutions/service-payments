using Application.Services.PaymentMethods;
using Domain.Services;

namespace Application.Factories;

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
