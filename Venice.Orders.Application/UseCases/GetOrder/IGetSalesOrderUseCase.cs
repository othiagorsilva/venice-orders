using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Venice.Orders.Application.DTOs;

namespace Venice.Orders.Application.UseCases.GetOrder
{
    public interface IGetOrderUseCase
    {
        Task<OrderDto?> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}