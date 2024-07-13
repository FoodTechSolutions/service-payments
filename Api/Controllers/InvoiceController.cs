using Domain.Boundaries.Invoices.CreateInvoice;
using Domain.Services;
using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("/invoice")]
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
                if (e.Message == "Order not found")
                    return NotFound(new
                    {
                        message = e.Message,
                        status = 404
                    });

                return BadRequest(
                    new ProblemDetails
                    {
                        Title = e.Message,
                        Status = StatusCodes.Status400BadRequest
                    }
                );
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetInvoiceByOrderId(
            [FromRoute] Guid id
        )
        {
            try
            {
                var response = await _invoiceService.GetInvoiceByOrderIdAsync(id);

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
