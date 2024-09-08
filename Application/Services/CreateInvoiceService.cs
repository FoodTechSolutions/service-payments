using Application.BackgroundServices.Models;
using Application.Services.Interface;
using Domain.Boundaries.Invoices.CreateInvoice;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CreateInvoiceService(IInvoiceRepository invoiceRepository) : ICreateInvoiceService
    {
        public async Task ProcessEvent(CreateInvoiceModel request)
        {
            var invoice = new Invoice(
                new Money(request.Amount, "BRL"),
                request.DueDate,
                request.OrderId);

            invoiceRepository.Add(invoice);
        }
    }
}
