using FluentValidation;
using Microsoft.Extensions.Logging;
using Venice.Orders.Application.Common.Interfaces;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Application.Orders.Messaging;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Application.UseCases.CreateOrder;
using Xunit;
using Moq;

namespace Venice.Orders.Test.Application
{
    public class CreateOrderUseCaseTest
    {
        private readonly Mock<IOrderRepository> _repoMock;
        private readonly Mock<IEventPublisher> _eventMock;
        private readonly Mock<IValidator<CreateOrderDto>> _validatorMock;
        private readonly Mock<ILogger<CreateOrderUseCase>> _loggerMock;
        private readonly CreateOrderUseCase _useCase;

        public CreateOrderUseCaseTest()
        {
            _repoMock = new Mock<IOrderRepository>();
            _eventMock = new Mock<IEventPublisher>();
            _validatorMock = new Mock<IValidator<CreateOrderDto>>();
            _loggerMock = new Mock<ILogger<CreateOrderUseCase>>();
            _useCase = new CreateOrderUseCase(
                _loggerMock.Object,
                _repoMock.Object,
                _eventMock.Object,
                _validatorMock.Object);
        }

        [Fact]
        public async Task CreateOrder()
        {
            var request = new CreateOrderDto(Guid.NewGuid(),
                new List<OrderItemDto>
                {
                    new("Encordoamento Guitarra", 2, 52)
                }
            );

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateOrderDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var result = await _useCase.ExecuteAsync(request, CancellationToken.None);

            _repoMock.Verify(r => r.InsertOneAsync(It.IsAny<Order>()), Times.Once);

            _eventMock.Verify(b => b.PublishAsync(It.IsAny<OrderCreatedEvent>()), Times.Once);

            Assert.NotEqual(Guid.Empty, result);
        }
    }
}