using Application.BackgroundServices.Models;
using Application.Configuration;
using Application.Helpers;
using Application.Services.Interface;
using Domain.Boundaries.Payments.ProcessPayment;
using Domain.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IRabbitMqService _rabbitMqService;

    public PaymentController(IPaymentService paymentService, IRabbitMqService rabbitMqService)
    {
        _paymentService = paymentService;
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost("/process")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        try
        {
            var response = await _paymentService.Process(request);

            if (response.Status.Equals(PaymentStatus.Failed))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Payment failed",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(
                new ProblemDetails
                {
                    Title = e.Message,
                    Status = StatusCodes.Status400BadRequest
                }
            );
        }
    }


    [HttpPost("Publish")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult> Publish([FromBody] RabbitMqExampleModel request)
    {

        var model = new RabbitMqPublishModel<RabbitMqExampleModel>
        {
            ExchangeName = EventConstants.RABBITMQ_EXAMPLE_EXCHANGE,
            RoutingKey = string.Empty,
            Message = request,
        };

        _rabbitMqService.Publish(model);

        return Ok("Ok");
    }
}
