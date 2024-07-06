using Domain.Boundaries.Invoices.CreateInvoice;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("/invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateInvoice(CreateInvoiceRequest request)
        {
            try
            {
                var response = await _invoiceService.CreateInvoiceAsync(request);

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(
                    new ProblemDetails
                    {
                        Title = e.Message,
                        Status = StatusCodes.Status400BadRequest
                    }
                );
            }
        }
    }
}
