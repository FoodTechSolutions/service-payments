using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BackgroundServices.Models;

public record CreateInvoiceModel
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
}

