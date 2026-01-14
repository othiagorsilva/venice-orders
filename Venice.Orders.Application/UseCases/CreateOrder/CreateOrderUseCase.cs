using FluentValidation;
using Microsoft.Extensions.Logging;
using Venice.Orders.Application.Common.Interfaces;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Application.Orders.Messaging;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;

namespace Venice.Orders.Application.UseCases.CreateOrder
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly ILogger<CreateOrderUseCase> _logger;
        private readonly IOrderRepository _repository;
        private readonly IEventPublisher _publisher;
        private readonly IValidator<CreateOrderDto> _validator;

        public CreateOrderUseCase(ILogger<CreateOrderUseCase> logger, IOrderRepository repository, IEventPublisher publisher, IValidator<CreateOrderDto> validator)
        {
            _logger = logger;
            _repository = repository;
            _publisher = publisher;
            _validator = validator;
        }

        public async Task<Guid> ExecuteAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(createOrderDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var salesOrder = new Order(createOrderDto.CustomerId);
            foreach (var item in createOrderDto.Items)
            {
                salesOrder.AddItem(item.Product, item.Quantity, item.UnitPrice);
            }

            try
            {
                await _repository.InsertOneAsync(salesOrder);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao persistir o pedido {OrderId}", salesOrder.Id);
                throw;
            }

            try
            {
                await _publisher.PublishAsync(new OrderCreatedEvent(salesOrder.Id, salesOrder.CustomerId, salesOrder.CreatedAt));
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex, "Pedido {OrderId} salvo, mas falha ao publicar evento no RabbitMQ", salesOrder.Id);
            }

            return salesOrder.Id;
        }
    }
}