using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Services;
using Domain.ValueObjects;
using System;

namespace Application.Services.PaymentMethods
{
    public class DebitPayment : IPaymentMethod
    {
        public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest paymentDetails)
        {
            var payment = new Payment(
                new Money(paymentDetails.Amount, "BRL"),
                paymentDetails.PaymentType,
                paymentDetails.InvoiceId
            );

            try
            {
                ValidateDebitCardInformation(paymentDetails);
                payment.Confirm();
            }
            catch (Exception e)
            {
                payment.Fail();
                throw;
            }

            return payment;
        }

        private void ValidateDebitCardInformation(ProcessPaymentRequest paymentRequest)
        {
            if (string.IsNullOrEmpty(paymentRequest.DebitCardNumber))
            {
                throw new ArgumentException("Debit card number is required.");
            }

            if (string.IsNullOrEmpty(paymentRequest.DebitCardExpiration))
            {
                throw new ArgumentException("Debit card expiration date is required.");
            }

            // Validar vencimento do cartao '10-2024'
            var expirationDate = paymentRequest.DebitCardExpiration.Split('-');
            if (expirationDate.Length != 2 ||
                expirationDate[0].Length != 2 ||
                expirationDate[1].Length != 4 ||
                !int.TryParse(expirationDate[0], out var month) ||
                !int.TryParse(expirationDate[1], out var year) ||
                month < 1 || month > 12 ||
                year < DateTime.Now.Year || year > DateTime.Now.Year + 10)
            {
                throw new ArgumentException("Invalid debit card expiration date.");
            }

            if (string.IsNullOrEmpty(paymentRequest.DebitCardCvv))
            {
                throw new ArgumentException("Debit card CVV is required.");
            }
        }
    }
}
