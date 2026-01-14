using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Application.UseCases.CreateOrder;
using Venice.Orders.Application.UseCases.GetOrder;

namespace Venice.Orders.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly ICreateOrderUseCase _createOrderUseCase;
        private readonly IGetOrderUseCase _getOrderUseCase;

        public OrderController(ICreateOrderUseCase createOrderUseCase, IGetOrderUseCase getOrderUseCase)
        {
            _createOrderUseCase = createOrderUseCase;
            _getOrderUseCase = getOrderUseCase;
        }

        /// <summary>
        /// Cria um novo pedido de venda
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreateOrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto request, CancellationToken cancellationToken)
        {
            var result = await _createOrderUseCase.ExecuteAsync(request, cancellationToken);

            return StatusCode((int)HttpStatusCode.Created, result);
        }

        /// <summary>
        /// Obtém os detalhes de um pedido (SQL + NoSQL)
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _getOrderUseCase.ExecuteAsync(id, ct);

            if (response == null)
                return NotFound(new { Message = $"Pedido {id} não encontrado." });

            return Ok(response);
        }
    }
}