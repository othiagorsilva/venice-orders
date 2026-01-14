using FluentValidation;
using Venice.Orders.Application.DTOs;

namespace Venice.Orders.Application.UseCases.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("O pedido deve ter pelo menos um item.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
            });
        }
    }
}