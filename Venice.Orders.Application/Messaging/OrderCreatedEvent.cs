using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Venice.Orders.Application.Orders.Messaging
{
    public record OrderCreatedEvent(
        Guid SalesOrderId,
        Guid CustomerId,
        DateTime CreatedAt
    );
}